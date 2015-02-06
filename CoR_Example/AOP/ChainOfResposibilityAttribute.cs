using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;

namespace CoR_Example {
    [Serializable]
    [IntroduceInterface(typeof (IHandler))]
    public class ChainOfResposibilityAttribute : InstanceLevelAspect, IHandler {
        private MethodInfo methodInfo;

        private readonly Type[] types;
        private readonly List<IHandler> handlers;

        public ChainOfResposibilityAttribute(params Type[] handlers) {
            types = handlers;
            this.handlers = new List<IHandler>();
        }

        public override void CompileTimeInitialize(Type type, AspectInfo aspectInfo) {
            methodInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Single();

            base.CompileTimeInitialize(type, aspectInfo);
        }

        public override void RuntimeInitializeInstance() {
            if (handlers.Count > 0) {
                return;
            }

            foreach (var handler in types) {
                var hdl = (IHandler) Activator.CreateInstance(handler);
                handlers.Add(hdl);
            }

            base.RuntimeInitializeInstance();
        }

        public void Handle(object o) {
            var handled = (bool) methodInfo.Invoke(Instance, new[] {o});

            if (!handled) {
                foreach (var handler in handlers) {
                    handler.Handle(o);
                }
            }
        }
    }
}