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
            Console.WriteLine(editingContact);

            editingContact.HomeNumber = "12234";
            service.Save(editingContact);
            Console.WriteLine(editingContact);

            editingContact.LastName = "Batman";
            service.Save(editingContact);
            Console.WriteLine(editingContact);

            service.Undo(editingContact);
            Console.WriteLine("Undo " + editingContact);

            service.Undo(editingContact);
            Console.WriteLine("Undo " + editingContact);

            service.Undo(editingContact);
            Console.WriteLine("Undo " + editingContact);

            service.Undo(editingContact);
            Console.WriteLine("Undo " + editingContact);

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