using System;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;

namespace AdapterExample.AOP {
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
}