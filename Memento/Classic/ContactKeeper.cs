using System.Collections.Generic;
using System.Linq;

namespace MementoExample.Classic {
    public class ContactKeeper {
        private readonly List<Contact.ContactMemento> mementoes = new List<Contact.ContactMemento>();

        public int LastVersion {
            get { return mementoes.Count - 1; }
        }

        public int CurrentVersion { get; set; }

        public void Set(Contact.ContactMemento memento) {
            if (CurrentVersion != LastVersion) {
                var oddMementoes = mementoes.Skip(CurrentVersion).Count();
                mementoes.RemoveRange(CurrentVersion, oddMementoes);
            }

            mementoes.Add(memento);
            CurrentVersion = LastVersion;
        }

        public Contact.ContactMemento GetCurrent() {
            if (CurrentVersion < 0) {
                CurrentVersion = 0;
            }

            if (CurrentVersion > LastVersion) {
                CurrentVersion = LastVersion;
            }

            return mementoes[CurrentVersion];
        }

        public Contact.ContactMemento GetPrevious() {
            var contactMemento = mementoes[CurrentVersion];
            CurrentVersion--;
            if (CurrentVersion < 0)
                CurrentVersion = 0;

            return contactMemento;
        }
    }
}