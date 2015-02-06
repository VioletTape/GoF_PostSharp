using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using PostSharp.Aspects;

namespace CompositeExample.AOP {
    [Serializable]
    public class NodeControl : MethodInterceptionAspect {
        private readonly List<Type> acceptableTypes;

        public NodeControl(List<Type> acceptableTypes) {
            this.acceptableTypes = acceptableTypes;
        }

        public override void OnInvoke(MethodInterceptionArgs args) {
            var type = args.Arguments.GetArgument(0).GetType();
            if (acceptableTypes.Contains(type)) {
                base.OnInvoke(args);
                return;
            }

            var message = string.Format("{0}.{1} can not accept {2}", args.Instance.GetType().Name, args.Method.Name, type.Name);
            throw new InvalidDataException(message);
        }

        public override bool CompileTimeValidate(MethodBase method) {
            var parameterInfos = method.GetParameters();
            return base.CompileTimeValidate(method);
        }
    }
}