using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApplication1123.NS;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;

namespace AdapterExample {
    internal class Program {
        private static void Main(string[] args) {
            var list = new List<object> {
                new Class1(),
                new Class1{Age = 22},
                new Class2(),
                new Class2{Name = "Dow"},
                new Class3(),
                new Class4()
            };

            foreach (var item in list) {
                Console.WriteLine(((IInfo) item).GetInfo());
            }

            Console.ReadLine();
        }
    }


    public interface IInfo {
        string GetInfo();
    }

    [Serializable]
    public class ProviderAspect : TypeLevelAspect, IAspectProvider {
        public IEnumerable<AspectInstance> ProvideAspects(object targetElement) {
            var targetType = (Type) targetElement;
            var aspectType = targetType.Assembly.GetTypes().SingleOrDefault(t => t.Name.Contains(targetType.Name + "Adapter"));

            if (aspectType != null) {
                var aspectConstruction = new ObjectConstruction(aspectType.GetConstructor(new Type[] {}));
                var aspects = new AspectInstance(targetElement, aspectConstruction, null);
                yield return aspects;
            }

//            if (type.Name == "Class1") {
//                yield return new AspectInstance(targetElement, new Class1Adapter());
//            }
//            if (type.Name == "Class2") {
//                yield return new AspectInstance(targetElement, new Class2Adapter());
//            }
//            if (type.Name == "Class3") {
//                yield return new AspectInstance(targetElement, new Class3Adapter());
//            }
        }
    }


    [IntroduceInterface(typeof (IInfo))]
    [Serializable]
    public class AdaptAspect : InstanceLevelAspect, IInfo {
        public virtual object GetValueFromParent() {
            return "no data";
        }

        public string GetInfo() {
            var type = Instance.GetType();
            return string.Format("Type {0} - New Method Get Value {1}", type.Name, GetValueFromParent());
        }
    }

    [Serializable]
    public class Class1Adapter : AdaptAspect {
        public override object GetValueFromParent() {
            return ((Class1) Instance).Age;
        }
    }

    [Serializable]
    public class Class2Adapter : AdaptAspect {
        public override object GetValueFromParent() {
            return ((Class2) Instance).Name;
        }
    }

    [Serializable]
    public class Class3Adapter : AdaptAspect {
        public override object GetValueFromParent() {
            return ((Class3) Instance).X;
        }
    }
}