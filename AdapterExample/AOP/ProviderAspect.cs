using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Aspects;
using PostSharp.Reflection;

namespace AdapterExample.AOP {
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
}