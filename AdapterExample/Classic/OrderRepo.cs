using System.Collections.Generic;
using System.Linq;

namespace AdapterExample.Classic {
    public class OrderRepo {
        private readonly DbAccessor dbAccessor;
        private readonly OrderAdapter orderAdapter;

        public OrderRepo() {
            dbAccessor = new DbAccessor();
            var table = dbAccessor.GetTable<Order>();
            orderAdapter = new OrderAdapter(table);
        }

        public List<Order> Get(Specification<Order> specification) {
            var result = dbAccessor.Select(orderAdapter.Convert(specification));

            return result;
        }
    }

    public class OrderAdapter : Adapter<Order> {
        public OrderAdapter(Table<Order> table) : base(table) {}
        public override IQueryable<Order> Convert(Specification<Order> specification) {
            var query = CommonConditions(specification);

            var forCustomer = specification.ExtractSupersetSpecification<OrderForCustomer>();
            if (forCustomer != null) {
                query = query.Where(i => i.Customer.Id == forCustomer.Customer.Id);
            }

            return query;
        }
    }

    public class OrderForCustomer : Specification<Order> {
        public Customer Customer { get; private set; }

        public OrderForCustomer(Customer customer) {
            Customer = customer;
        }

        public override bool IsSatisfiedBy(Order obj) {
            return obj.Customer == Customer;
        }
    }
}