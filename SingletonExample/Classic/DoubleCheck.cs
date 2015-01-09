using System.Threading;

namespace SingletonExample.Classic {
    public sealed class MySingleton {
        private static readonly object lockObj = new object();
        private static MySingleton singleton;
        
        public static MySingleton Instance {
            get {
                if (singleton == null) {
                    lock (lockObj) {
                        Thread.MemoryBarrier();
                        if (singleton == null) {
                            singleton = new MySingleton();
                        }
                    }
                }
                return singleton;
            }
        }

        private MySingleton() {}
    }

    public class SomeClientClass {
        public void Foo() {
            var mySingleton = MySingleton.Instance;

        }
    }
}