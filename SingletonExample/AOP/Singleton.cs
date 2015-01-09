using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp;
using PostSharp.Aspects;
using PostSharp.Extensibility;

namespace SingletonExample.AOP {
    [Serializable]
    public class Singleton : TypeLevelAspect, IAspectProvider {
        public override bool CompileTimeValidate(Type type) {
            var memberInfos = type.GetProperties(BindingFlags.Static | BindingFlags.Public);

            var gotErrors = false;

            var instanceFieldExists = memberInfos.Any(m => m.DeclaringType == type);
            if (!instanceFieldExists) {
                var messageText = string.Format("There should be static property with class type in the {0} class", type.Name);
                var message = new Message(SeverityType.Error, "f001", messageText, "", "file", "", 1, 1, null);
                Message.Write(message);
                gotErrors = true;
            }

            var singleInstanceFields = memberInfos.Count(m => m.DeclaringType == type);
            if (singleInstanceFields != 1) {
                var messageText = string.Format("There should be only one static property with class type in the {0} class (was found {1})", type.Name, singleInstanceFields);
                var message = new Message(SeverityType.Error, "f001", messageText, "", "file", "", 1, 1, null);
                Message.Write(message);
                gotErrors = true;
            }

            var constructorInfos = type.GetConstructors();
            if (constructorInfos.Any()) {
                var messageLocation = MessageLocation.Of(constructorInfos.First());
                var messageText = string.Format("All constructors for the {0} class should be not public", type.Name);
                var message = new Message(messageLocation, SeverityType.Error, "c001", messageText, "", "file", null);
                Message.Write(message);
                gotErrors = true;
            }
            return gotErrors;
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