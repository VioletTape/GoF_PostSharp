using System.Collections.Generic;

namespace CoR_Example.Classic.InternalScheme {
    public abstract class Handler {
        protected List<IDeliveryService> services;

        public void CalcDelivery(Address address) {
            foreach (var service in services) {
                var calcDelivery = service.CalcDelivery(address);
                if (calcDelivery > 0) {
                    address.DeliveryPrice = calcDelivery;
                    return;
                }
            }

            ProcessHandlers(address);
        }

        protected abstract void ProcessHandlers(Address address);
    }


    public class NullHandler : Handler {
        protected override void ProcessHandlers(Address address) {}
    }


    public class LocalDeliveryHandler : Handler {
        public LocalDeliveryHandler() {
            services = new List<IDeliveryService> {
                new LocalDeliveryService(),
            };
        }

        protected override void ProcessHandlers(Address address) {
            new UsaDeliveryHandler().CalcDelivery(address);
        }
    }


    public class UsaDeliveryHandler : Handler {
        public UsaDeliveryHandler() {
            services = new List<IDeliveryService> {
                new CountryManDelivery(),
                new AllUsaDelivery(),
                new LittlePonyExpress()
            };
        }

        protected override void ProcessHandlers(Address address) {
            new GlobalDeliveryHandler().CalcDelivery(address);
        }
    }


    public class GlobalDeliveryHandler : Handler {
        public GlobalDeliveryHandler() {
            services = new List<IDeliveryService> {
                new LittlePonyExpress(),
                new WorldExpress()
            };
        }

        protected override void ProcessHandlers(Address address) {}
    }
}