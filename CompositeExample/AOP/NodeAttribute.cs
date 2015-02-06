using System;
using System.Collections.Generic;
using PostSharp.Aspects;

namespace CompositeExample.AOP {
    [Serializable]
    public class NodeAttribute : TypeLevelAspect, IAspectProvider {
        public static int TotalElements;
        public static Dictionary<Type, List<Type>> Refs = new Dictionary<Type, List<Type>>();

        public Type Parent { get; set; }

        public override void CompileTimeInitialize(Type type, AspectInfo aspectInfo) {
            base.CompileTimeInitialize(type, aspectInfo);
        }

        public IEnumerable<AspectInstance> ProvideAspects(object targetElement) {
            var type = ((Type) targetElement);

            var methodInfo = type.GetMethod("Add");
            if (methodInfo != null && Refs.ContainsKey(type)) {
                yield return new AspectInstance(methodInfo, new NodeControl(Refs[type]));
            }
        }
    }
}