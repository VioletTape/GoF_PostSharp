using System.Collections.Generic;
using System.Linq;

namespace MementoExample.ClassicSimple {
//     public class ContactService<T> where T: class {
//        private readonly List<T> contacts = new List<T>();
//        private readonly Dictionary<T, Keeper<T>> keepers = new Dictionary<T, Keeper<T>>();
//
//        public void Save(T contact) {
//            if (!contacts.Contains(contact)) {
//                contacts.Add(contact);
//                keepers[contact] = new Keeper<T>();
//            }
//
//            keepers[contact].Set(contact.GetMemento());
//        }
//
//        public Contact Find(string value) {
//            return null;
//        }
//
//        public void Undo(Contact contact) {
//            var memento = keepers[contact].GetPrevious();
//            contact.SetMemento(memento);
//        }
//
//        public void Redo(Contact contact) {
//            keepers[contact].CurrentVersion++;
//            var memento = keepers[contact].GetCurrent();
//            contact.SetMemento(memento);
//        }
//    }

    public class Keeper<T> where T : class {
        private readonly List<T> mementoes = new List<T>();

        public int LastVersion {
            get { return mementoes.Count - 1; }
        }

        public int CurrentVersion { get; set; }

        public void Set(T memento) {
            if (CurrentVersion != LastVersion) {
                var oddMementoes = mementoes.Skip(CurrentVersion).Count();
                mementoes.RemoveRange(CurrentVersion, oddMementoes);
            }

            mementoes.Add(memento);
            CurrentVersion = LastVersion;
        }

        public T GetCurrent() {
            if (CurrentVersion < 0) {
                CurrentVersion = 0;
            }

            if (CurrentVersion > LastVersion) {
                CurrentVersion = LastVersion;
            }

            return mementoes[CurrentVersion];
        }

        public T GetPrevious() {
            var contactMemento = mementoes[CurrentVersion];
            CurrentVersion--;
            if (CurrentVersion < 0)
                CurrentVersion = 0;

            return contactMemento;
        }
    }
}