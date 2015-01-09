using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;

namespace MementoExample {
    [Serializable]
    [IntroduceInterface(typeof (IMemento))]
    public class MementoAttribute : InstanceLevelAspect, IMemento {
        private List<LocationInfo> memberInfos;
        private Type type;
        private readonly Stack<Dictionary<LocationInfo, object>> stack = new Stack<Dictionary<LocationInfo, object>>();


        public override void CompileTimeInitialize(Type type, AspectInfo aspectInfo) {
            this.type = type;
            const MemberTypes memberTypes = MemberTypes.Field;

            const BindingFlags bindingFlags = BindingFlags.Instance
                                              | BindingFlags.Public
                                              | BindingFlags.NonPublic;

            memberInfos = type.FindMembers(memberTypes,
                bindingFlags,
                null,
                null)
                              .ToList().ConvertAll(i => new LocationInfo((FieldInfo) i));

            base.CompileTimeInitialize(type, aspectInfo);
        }


        public void Save() {
            var dictionary = new Dictionary<LocationInfo, object>();
            foreach (var locationInfo in memberInfos) {
                dictionary[locationInfo] = locationInfo.GetValue(Instance);
            }
            stack.Push(dictionary);
        }

        public void Undo() {
            if (stack.Count == 0) {
                return;
            }

            var dictionary = stack.Pop();
            foreach (var locationInfo in memberInfos) {
                locationInfo.SetValue(Instance, dictionary[locationInfo]);
            }
        }
    }
}