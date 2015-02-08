using System.Collections.Generic;

namespace AdapterExample.Classic {
    public class CustomerRepo {
        private readonly DbAccessor dbAccessor;
        private readonly CustomerAdapter customerAdapter;

        public CustomerRepo() {
            dbAccessor = new DbAccessor();
            var table = dbAccessor.GetTable<Customer>();
            customerAdapter = new CustomerAdapter(table);
        }

        public List<Customer> Get(Specification<Customer> specification) {
            var result = dbAccessor.Select(customerAdapter.Convert(specification));

            return result;
        }
    }

    public class CustomerNameStartFrom : Specification<Customer> {
        public string NameStartsFrom { get; private set; }

        public CustomerNameStartFrom(string nameStartsFrom) {
            NameStartsFrom = nameStartsFrom;
        }

        public override bool IsSatisfiedBy(Customer obj) {
            return obj.Name.StartsWith(NameStartsFrom);
        }
    }

    public class TopSpecification<T> : Specification<T> {
        public int Top { get; private set; }

        public TopSpecification(int top) {
            Top = top;
        }

        public override bool IsSatisfiedBy(T obj) {
            return true;
        }
    }
}