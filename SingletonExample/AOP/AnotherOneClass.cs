namespace SingletonExample.AOP {
    [Singleton]
    public class AnotherOneClass {
        public static AnotherOneClass Instance { get; set; }
        private AnotherOneClass() {}
    }
}