namespace AdapterExample.Classic {
    public class ClientCode {
        public ClientCode() {
            var customers = new CustomerRepo().Get(new TopSpecification<Customer>(10));
            var orders = new OrderRepo().Get(new TopSpecification<Order>(10) & new OrderForCustomer(customers[0]));
        }
    }
}