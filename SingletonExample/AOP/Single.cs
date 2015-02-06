using System;
using System.Reflection;
using PostSharp;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;

namespace SingletonExample.AOP {
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
                var messageLocation = MessageLocation.Of(propertyInfo);
                var messageText = string.Format("Attribute applied to the property {1} should return {0} type", type.Name, locationInfo.Name);
                var message = new Message(messageLocation, SeverityType.Error, "f001", messageText, "", "file", null);
                Message.Write(message);
            }

            return base.CompileTimeValidate(locationInfo);
        }
    }
}