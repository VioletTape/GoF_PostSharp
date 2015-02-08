using System;
using System.Collections.Generic;
using AdapterExample.NS;

namespace AdapterExample {
    internal class Program {
        private static void Main(string[] args) {
            var list = new List<object> {
                new Class1(),
                new Class1{Age = 22},
                new Class2(),
                new Class2{Name = "Dow"},
                new Class3(),
                new Class4()
            };

            foreach (var item in list) {
                Console.WriteLine(((IInfo) item).GetInfo());
            }

            Console.ReadLine();
        }
    }
}