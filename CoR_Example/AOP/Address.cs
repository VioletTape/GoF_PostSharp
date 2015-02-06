namespace CoR_Example {
    public class Address {
        public string Country { get; set; }
        public string State { get; set; }
        public decimal DeliveryPrice { get; set; }

        public void SetDeliveryPrice(decimal price) {
            DeliveryPrice = price;
        }
    }
}