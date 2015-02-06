using System;
using CompositeExample.AOP;

namespace CompositeExample {
    internal class Program {
        private static void Main(string[] args) {
            var topLevel1 = new TopLevel();

            topLevel1.Add(new LowLevel2());
            Console.WriteLine(topLevel1.Count());

            topLevel1.Add(new MiddleLevel());
            Console.WriteLine(topLevel1.Count());


            Console.ReadLine();
        }
    }
}