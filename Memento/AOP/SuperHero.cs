using System;

namespace MementoExample {
    [Memento]
    public class SuperHero {
        private Guid id ;
        public int Age { get; set; }
        public string Name { get; set; }

        public override string ToString() {
            return string.Format("{0} - {1}", Age, Name);
        }
    }
}