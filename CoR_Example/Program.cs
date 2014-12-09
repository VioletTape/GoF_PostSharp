using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;

namespace CoR_Example {
    internal class Program {
        private static void Main(string[] args) {
            var addresses = new List<Address> {
                new Address {
                    Country = "Canada"
                },
                new Address {
                    Country = "USA"
                }
            };

            foreach (var address in addresses) {
                ((IHandler) new CountryManDelivery()).Handle(address);
                Console.WriteLine("Delivery price in {0} is {1}", address.Country, address.DeliveryPrice);
            }

            Console.ReadLine();
        }
    }


    public interface IHandler {
        void Handle(object o);
    }

    [ChainOfResposibility(typeof (LittlePonyExpress))]
    public class CountryManDelivery {
        public bool CalcDelivery(Address address) {
            if (address.Country == "Canada") {
                address.SetDeliveryPrice(1);
                return true;
            }
            return false;
        }
    }

    [ChainOfResposibility]
    public class LittlePonyExpress {
        public bool CalcWorldwideDelivery(Address address) {
            address.SetDeliveryPrice(2);
            return true;
        }
    }






    [Serializable]
    [IntroduceInterface(typeof (IHandler))]
    public class ChainOfResposibilityAttribute : InstanceLevelAspect, IHandler {
        private MethodInfo methodInfo;

        private readonly Type[] types;
        private readonly List<IHandler> handlers;

        public ChainOfResposibilityAttribute(params Type[] handlers) {
            types = handlers;
            this.handlers = new List<IHandler>();
        }

        public override void CompileTimeInitialize(Type type, AspectInfo aspectInfo) {
            methodInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Single();

            base.CompileTimeInitialize(type, aspectInfo);
        }

        public override void RuntimeInitializeInstance() {
            if(handlers.Count > 0)
                return;

            foreach (var handler in types) {
                var hdl = (IHandler) Activator.CreateInstance(handler);
                handlers.Add(hdl);
            }

            base.RuntimeInitializeInstance();
        }

        public void Handle(object o) {
            var handled = (bool) methodInfo.Invoke(Instance, new[] {o});

            if (!handled) {
                foreach (var handler in handlers) {
                    handler.Handle(o);
                }
            }
        }
    }


    public class Address {
        public string Country { get; set; }
        public string State { get; set; }
        public decimal DeliveryPrice { get; set; }

        public void SetDeliveryPrice(decimal price) {
            DeliveryPrice = price;
        }
    }
}