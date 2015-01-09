using System;

namespace MementoExample.AOP {
    [Memento]
    public class SuperHero {
        public int Age { get; set; }
        public string Name { get; set; }

        public override string ToString() {
            return string.Format("{0} - {1}", Age, Name);
        }
    }
}