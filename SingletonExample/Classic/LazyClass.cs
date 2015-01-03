using System;
using System.Threading;

namespace SingletonExample.Classic {

    public sealed class SingletonLazy {
        private static Lazy<SingletonLazy> instance = new Lazy<SingletonLazy>(() => new SingletonLazy());

        public static SingletonLazy Instance {
            get { return instance.Value; }
        }

        private SingletonLazy() {}
    }
}