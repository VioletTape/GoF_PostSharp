using System;

namespace CoR_Example.Classic.InternalScheme {
    public class Address {
        public string Country { get; set; }
        public string State { get; set; }
        public decimal DeliveryPrice { get; set; }


        public event Action DeliveryPriceChanged;
        public void SetDeliveryPrice(decimal price) {
            DeliveryPriceChanged();
        }
    }
}