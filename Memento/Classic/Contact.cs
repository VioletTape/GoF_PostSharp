using System;

namespace MementoExample.Classic {
    public class Contact  {
        private Guid guid = Guid.NewGuid();

        public Guid Guid {
            get { return guid; } 
        }

        public Type ContactType { get; private set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CellNumber { get; set; }
        public string HomeNumber { get; set; }
        public string Note { get; set; }

        public Contact(Type type) {
            ContactType = type;
        }

        public enum Type {
            Private,
            Public
        }

        protected bool Equals(Contact other) {
            return guid.Equals(other.guid);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != GetType()) {
                return false;
            }
            return Equals((Contact) obj);
        }

        public override int GetHashCode() {
            return guid.GetHashCode();
        }

        public override string ToString() {
            return string.Format("{0} {1} {2} {3} {4} {5}", guid, ContactType, Name, LastName, CellNumber, HomeNumber);
        }

        public ContactMemento GetMemento() {
            return new ContactMemento {
                                          Guid = guid,
                                          ContactType = ContactType,
                                          Name = Name,
                                          LastName = LastName,
                                          CellNumber = CellNumber,
                                          HomeNumber = HomeNumber,
                                          Note = Note
                                      };
        }

        public void SetMemento(ContactMemento memento) {
            guid = memento.Guid;
            ContactType = memento.ContactType;
            Name = memento.Name;
            LastName = memento.LastName;
            CellNumber = memento.CellNumber;
            HomeNumber = memento.HomeNumber;

        }

        public class ContactMemento {
            public Guid Guid;
            public Type ContactType;
            public string Name;
            public string LastName;
            public string CellNumber;
            public string HomeNumber;
            public string Note;
        }
    }
}