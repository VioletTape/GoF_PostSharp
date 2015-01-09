using System;

namespace MementoExample {
    internal class Program {
        private static void Main(string[] args) {
            var superHero = new SuperHero();
            var memento = ((IMemento) superHero);

            superHero.Age = 12;
            superHero.Name = "Bruce Wayne";
            Console.WriteLine(superHero);
            memento.Save();

            superHero.Age = 34;
            superHero.Name = "Batman";
            Console.WriteLine(superHero);
            memento.Save();

            superHero.Age = 41;
            superHero.Name = "Dark Knight";
            Console.WriteLine(superHero);
           

            memento.Undo();
            Console.WriteLine("Undo " + superHero);
            memento.Undo();
            Console.WriteLine("Undo " + superHero);
            memento.Undo();
            Console.WriteLine("Undo " + superHero);
            memento.Undo();
            Console.WriteLine("Undo " + superHero);
            memento.Undo();
            Console.WriteLine("Undo " + superHero);

            Console.ReadLine();

        }
    }
}