using System.Collections.Generic;
using MementoExample.Classic;

namespace MementoExample.ClassicSimple {
// ------------------------ 
// Service without interface Memento<T>
// --------------------
//     public class KeeperService {
//        private readonly List<SuperHero> hero = new List<SuperHero>();
//        private readonly Dictionary<SuperHero, Keeper<SuperHeroMemento>> keepers = new Dictionary<SuperHero, Keeper<SuperHeroMemento>>();
//
//        public void Save(SuperHero hero) {
//            if (!this.hero.Contains(hero)) {
//                this.hero.Add(hero);
//                keepers[hero] = new Keeper<SuperHeroMemento>();
//            }
//
//            keepers[hero].Set(hero.CreateMemento()); 
//        }
//
//        public Contact Find(string value) {
//            return null;
//        }
//
//        public void Undo(SuperHero hero) {
//            var memento = keepers[hero].GetPrevious();
//            hero.SetMemento(memento);
//        }
//
//        public void Redo(SuperHero hero) {
//            keepers[hero].CurrentVersion++;
//            var memento = keepers[hero].GetCurrent();
//            hero.SetMemento(memento);
//        }
//     }
    
    public class KeeperService<T, TMemento> where T : IMemento<TMemento> where TMemento : class {
        private readonly List<T> items = new List<T>();
        private readonly Dictionary<T, Keeper<TMemento>> keepers = new Dictionary<T, Keeper<TMemento>>();

        public void Save(T item) {
            if (!this.items.Contains(item)) {
                this.items.Add(item);
                keepers[item] = new Keeper<TMemento>();
            }

            keepers[item].Set(item.CreateMemento()); 
        }

        public Contact Find(string value) {
            return null;
        }

        public void Undo(T item) {
            var memento = keepers[item].GetPrevious();
            item.SetMemento(memento);
        }

        public void Redo(T item) {
            keepers[item].CurrentVersion++;
            var memento = keepers[item].GetCurrent();
            item.SetMemento(memento);
        }
     }
}