namespace SingletonExample.Classic {
    public sealed class SingletonStatic {
         private static readonly SingletonStatic instance = new SingletonStatic();

        public static SingletonStatic Instance {
            get { return instance; }
        }

        private SingletonStatic() {}
    }
}