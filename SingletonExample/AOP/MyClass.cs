namespace SingletonExample.AOP {
    [Singleton]
    public sealed class MyClass {
        public int Counter;

        public static MyClass Instance { get; private set; }
        private MyClass() {}

        public int Foo() {
            return ++Counter;
        }
    }
}