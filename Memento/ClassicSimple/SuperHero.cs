namespace MementoExample.ClassicSimple {
    public class SuperHero : IMemento<SuperHeroMemento> {
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

    public interface IMemento<T> where T : class {
        T CreateMemento();
        void SetMemento(T memento);
    }
}