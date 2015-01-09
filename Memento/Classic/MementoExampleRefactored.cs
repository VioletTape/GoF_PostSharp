using System;

namespace MementoExample.Classic {
    public class MementoExampleRefactored {
        public MementoExampleRefactored() {
            var contact = new Contact(Contact.Type.Private);
            contact.Name = "Bruce";
            contact.LastName = "Waine";

            var service = new ContactService();
            service.Save(contact);

            Print(contact);

            var editingContact = contact;
            editingContact.CellNumber = "555-23-11";
            service.Save(editingContact);
            Print(editingContact);

            editingContact.HomeNumber = "12234";
            service.Save(editingContact);
            Print(editingContact);

            editingContact.LastName = "Batman";
            service.Save(editingContact);
            Print(editingContact);

            Console.WriteLine("");
            Console.WriteLine("Undo");
            service.Undo(editingContact);
            Print(editingContact);

            service.Undo(editingContact);
            Print(editingContact);

            service.Undo(editingContact);
            Print(editingContact);

            service.Undo(editingContact);
            Print(editingContact);

            Console.WriteLine("");
            Console.WriteLine("Redo");
            service.Redo(editingContact);
            Print(editingContact);

            service.Redo(editingContact);
            Print(editingContact);

            
        }

        private void Print(Contact contact) {
            Console.WriteLine(contact);
        }
    }
}