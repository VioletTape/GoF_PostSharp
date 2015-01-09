using System.Collections.Generic;
using MementoExample.Classic;

namespace MementoExample.ClassicSimple {
     public class ContactService {
        private readonly List<SuperHero> contacts = new List<SuperHero>();
        private readonly Dictionary<SuperHero, Keeper<SuperHeroMemento>> keepers = new Dictionary<SuperHero, Keeper<SuperHeroMemento>>();

        public void Save(SuperHero contact) {
            if (!contacts.Contains(contact)) {
                contacts.Add(contact);
                keepers[contact] = new Keeper<SuperHeroMemento>();
            }

            keepers[contact].Set(contact.CreateMemento());
        }

        public Contact Find(string value) {
            return null;
        }

        public void Undo(SuperHero contact) {
            var memento = keepers[contact].GetPrevious();
            contact.SetMemento(memento);
        }

        public void Redo(SuperHero contact) {
            keepers[contact].CurrentVersion++;
            var memento = keepers[contact].GetCurrent();
            contact.SetMemento(memento);
        }
    }
}