using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AdapterExample.Classic {
    public class DbAccessor {
        public Table<T> GetTable<T>() {
            return new Table<T>();
        }

        public List<T> Select<T>(IQueryable<T> query) {
            // access to db, parsing and so on
            var result = query.ToList();

            return result;
        }
    }

    public class Table<T> : IQueryable<T> {
        public IEnumerator<T> GetEnumerator() {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public Expression Expression { get; private set; }
        public Type ElementType { get; private set; }
        public IQueryProvider Provider { get; private set; }
    }
}