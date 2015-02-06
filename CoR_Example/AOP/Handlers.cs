namespace CoR_Example.AOP {
    [ChainOfResposibility(typeof (LittlePonyExpress))]
    public class CountryManDelivery {
        public bool CalcDelivery(Address address) {
            if (address.Country == "Canada") {
                address.SetDeliveryPrice(1);
                return true;
            }
            return false;
        }
    }

    [ChainOfResposibility]
    public class LittlePonyExpress {
        public bool CalcWorldwideDelivery(Address address) {
            address.SetDeliveryPrice(2);
            return true;
        }
    }
}