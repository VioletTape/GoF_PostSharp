using System.Collections.Generic;

namespace MementoExample.Classic {
    public class ContactService {
        private readonly List<Contact> contacts = new List<Contact>();
        private readonly Dictionary<Contact, ContactKeeper> keepers = new Dictionary<Contact, ContactKeeper>();

        public void Save(Contact contact) {
            if (!contacts.Contains(contact)) {
                contacts.Add(contact);
                keepers[contact] = new ContactKeeper();
            }

            keepers[contact].Set(contact.GetMemento());
        }

        public Contact Find(string value) {
            return null;
        }

        public void Undo(Contact contact) {
            var memento = keepers[contact].GetPrevious();
            contact.SetMemento(memento);
        }

        public void Redo(Contact contact) {
            keepers[contact].CurrentVersion++;
            var memento = keepers[contact].GetCurrent();
            contact.SetMemento(memento);
        }
    }
}