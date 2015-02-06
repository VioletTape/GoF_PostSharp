using System;

namespace CoR_Example.Classic.InternalScheme {
    public class Client {
        public Client() {
            var address = new Address();
            address.DeliveryPriceChanged += () => {
                if (address.DeliveryPrice > 0) {
                    Console.WriteLine("Delivery price is " + address.DeliveryPrice);
                }
                else {
                    Console.WriteLine("Can not deliver you postage");
                }
            };

            new LocalDeliveryHandler().CalcDelivery(address);
        }
    }

   
}