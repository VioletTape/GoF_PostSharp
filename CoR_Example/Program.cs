using System;
using System.Collections.Generic;
using CoR_Example.AOP;

namespace CoR_Example {
    internal class Program {
        private static void Main(string[] args) {
            var addresses = new List<Address> {
                                                  new Address {
                                                                  Country = "Canada"
                                                              },
                                                  new Address {
                                                                  Country = "USA"
                                                              }
                                              };

            foreach (var address in addresses) {
                ((IHandler) new CountryManDelivery()).Handle(address);
                Console.WriteLine("Delivery price in {0} is {1}", address.Country, address.DeliveryPrice);
            }

            Console.ReadLine();
        }
    }
}