namespace MementoExample.ClassicSimple {
    public class SuperHero {
        public int Age { get; set; }
        public string Name { get; set; }

        public override string ToString() {
            return string.Format("{0} - {1}", Age, Name);
        }

        public SuperHeroMemento CreateMemento() {
            return new SuperHeroMemento {
                                            Age = Age,
                                            Name = Name
                                        };
        }

        public void SetMemento(SuperHeroMemento memento) {
            Age = memento.Age;
            Name = memento.Name;
        }
    }
}