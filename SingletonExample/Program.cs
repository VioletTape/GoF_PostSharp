using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;

namespace SingletonExample {
    internal class Program {
        private static void Main(string[] args) {
            var myClass = MyClass.Instance;
            myClass.Counter = 5;
            myClass.Foo();
            Console.WriteLine(myClass.Counter);


            var myClass2 = MyClass.Instance;
            Console.WriteLine(myClass2.Counter);

            Console.ReadLine();
        }
    }


    [SingletonClass]
    public class MyClass {
        public int Counter;

        public static MyClass Instance { get; private set; }

        private MyClass() {}

        public int Foo() {
            return ++Counter;
        }
    }

    [SingletonClass]
    public class AnotherOneClass {
        public static AnotherOneClass Instance { get; set; }

        private AnotherOneClass() {}
    }




    [Serializable]
    public class Single : LocationInterceptionAspect {
        private static object instance;

        public override void OnGetValue(LocationInterceptionArgs args) {
            if (instance == null) {
                var type = args.Location.DeclaringType;
                var constructor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, new ParameterModifier[] {});
                instance = constructor.Invoke(new object[] {});
            }

            args.Value = instance;
        }

        public override bool CompileTimeValidate(LocationInfo locationInfo) {
            var propertyInfo = locationInfo.PropertyInfo;
            var type = locationInfo.DeclaringType;


            var instanceFieldExists = propertyInfo.PropertyType.UnderlyingSystemType == type;
            if (!instanceFieldExists) {
                var messageText = string.Format("Attribute applied to the property {1} should return {0} type", type.Name, locationInfo.Name);
                var message = new Message(SeverityType.Error, "f001", messageText, "", "file", "", 1, 1, null);
                Message.Write(message);
            }

            return base.CompileTimeValidate(locationInfo);
        }
    }


    [Serializable]
    public class SingletonClass : TypeLevelAspect, IAspectProvider {
        public override bool CompileTimeValidate(Type type) {
            var memberInfos = type.GetMembers(BindingFlags.Static | BindingFlags.Public);

            var instanceFieldExists = memberInfos.Any(m => m.DeclaringType == type);
            if (!instanceFieldExists) {
                var messageText = string.Format("There should be static property with class type in the {0} class", type.Name);
                var message = new Message(SeverityType.Error, "f001", messageText, "", "file", "", 1, 1, null);
                Message.Write(message);
            }

            var constructorInfos = type.GetConstructors();
            if (constructorInfos.Any()) {
                var messageLocation = MessageLocation.Of(constructorInfos.First());
                var messageText = string.Format("All constructors for the {0} class should be not public", type.Name);
                var message = new Message(messageLocation, SeverityType.Error, "c001", messageText, "", "file", null);
                Message.Write(message);
            }
            return !constructorInfos.Any();
        }

        public IEnumerable<AspectInstance> ProvideAspects(object targetElement) {
            var type = ((Type) targetElement);

            var memberInfos = type.GetProperties(BindingFlags.Static | BindingFlags.Public);

            foreach (var targetPropertyInfo in memberInfos) {
                if (targetPropertyInfo.PropertyType.UnderlyingSystemType == type) {
                    var aspects = new AspectInstance(targetPropertyInfo, new Single());
                    yield return aspects;
                }
            }
        }
    }
}