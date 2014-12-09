using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;

namespace FactoryExample {
    internal class Program {
        private static void Main(string[] args) {
            var solderFactory = new SolderFactory();
            Console.WriteLine(solderFactory.Get<Infantry>());
            Console.WriteLine(solderFactory.Get<Trooper>());

            Console.ReadLine();
        }
    }


    public abstract class Soldier {}

    [FactoryItem("Trooper")]
    public class Trooper : Soldier {}

    [FactoryItem("Infantry")]
    public class Infantry : Soldier {}

    public class SolderFactory {
        [FactoryMethod(typeof (Soldier))]
        public Soldier Get<T>() where T : Soldier {
            // intentinally left blank with null result
            return null;
        }
    }

    public class SolderFactory2 {
        [FactoryMethod2(typeof (Soldier))]
        public Soldier Get(string name) {
            // intentinally left blank with null result
            return null;
        }
    }

    public abstract class Tank {}

    public class HeavyTank : Tank {}

    public class LightTank : Tank {}

    public class TankFactory {
        [FactoryMethod(typeof (Tank))]
        public Tank Get<T>() where T : Tank {
            // intentinally left blank with null result
            return null;
        }
    }


    [Serializable]
    public class FactoryMethodAttribute : OnMethodBoundaryAspect {
        private readonly Type generatingFamily;

        public FactoryMethodAttribute(Type generatingFamily) {
            this.generatingFamily = generatingFamily;
        }

        public override void OnSuccess(MethodExecutionArgs args) {
            var type = args.Method.GetGenericArguments().SingleOrDefault();

            if (type.BaseType != generatingFamily) {
                base.OnSuccess(args);
                return;
            }

            var constructorInfo = type.GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new Type[] {},
                new ParameterModifier[] {});
            var invoke = constructorInfo.Invoke(new object[] {});
            args.ReturnValue = invoke;

            base.OnSuccess(args);
        }
    }


    [Serializable]
    public class FactoryMethod2Attribute : OnMethodBoundaryAspect {
        private readonly Type generatingFamily;

        private static readonly Dictionary<string, ConstructorInfo> ctrInfos
            = new Dictionary<string, ConstructorInfo>();

        public FactoryMethod2Attribute(Type generatingFamily) {
            this.generatingFamily = generatingFamily;
        }

        public override void OnSuccess(MethodExecutionArgs args) {
            var invoke = ctrInfos[args.Arguments[0].ToString()]
                .Invoke(new object[] {});
            args.ReturnValue = invoke;

            base.OnSuccess(args);
        }

        public override void RuntimeInitialize(MethodBase method) {
            var assembly = method.DeclaringType.Assembly;
            foreach (var type in assembly.GetTypes()) {
                if (type.BaseType == generatingFamily) {
                    var value = type.GetCustomAttribute<FactoryItemAttribute>().Id;
                    var constructorInfo = type.GetConstructor(
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                        null, new Type[] {}, new ParameterModifier[] {});
                    ctrInfos.Add(value, constructorInfo);
                }
            }
            base.RuntimeInitialize(method);
        }
    }


    [Serializable]
    public class FactoryItemAttribute : Attribute {
        public string Id;

        public FactoryItemAttribute(string id) {
            Id = id;
        }
    }
}