using System;
using AdapterExample.NS;

namespace AdapterExample.AOP {
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