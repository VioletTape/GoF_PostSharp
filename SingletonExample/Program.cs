using System;
using SingletonExample.AOP;

namespace SingletonExample {
    internal class Program {
        private static void Main(string[] args) {
            var myClass = MyClass.Instance;
            myClass.Counter = 5;
            myClass.Foo();
            Console.WriteLine(myClass.Counter);


            var myClass2 = MyClass.Instance;
            Console.WriteLine(myClass2.Counter);

            Console.ReadLine();
        }
    }
}