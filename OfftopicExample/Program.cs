using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;

namespace OfftopicExample {
    internal class Program {
        private static void Main(string[] args) {}
    }

    [MulticastAttributeUsage(MulticastTargets.Class, Inheritance = MulticastInheritance.Strict)]
    [Serializable]
    public sealed class AutoDataContractAttribute : TypeLevelAspect, IAspectProvider {
        // This method is called at build time and should just provide other aspects.
        public IEnumerable<AspectInstance> ProvideAspects(object targetElement) {
            var targetType = (Type) targetElement;

            var introduceDataContractAspect =
                new CustomAttributeIntroductionAspect(
                    new ObjectConstruction(typeof (DataContractAttribute).GetConstructor(Type.EmptyTypes)));
            var introduceDataMemberAspect =
                new CustomAttributeIntroductionAspect(
                    new ObjectConstruction(typeof (DataMemberAttribute).GetConstructor(Type.EmptyTypes)));


            // Add the DataContract attribute to the type.
            yield return new AspectInstance(targetType, introduceDataContractAspect);

            // Add a DataMember attribute to every relevant property.
            foreach (var property in
                targetType.GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance)) {
                if (property.CanWrite && !property.IsDefined(typeof (NotDataMemberAttribute), false)) {
                    yield return new AspectInstance(property, introduceDataMemberAspect);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotDataMemberAttribute : Attribute {}
}