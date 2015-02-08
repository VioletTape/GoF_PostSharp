using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AdapterExample.Classic {
    public class NullSpecification<T> : Specification<T> {
        public override bool IsSatisfiedBy(T obj) {
            return true;
        }
    }

    public class AndSpecification<T> : CompositeSpecification<T> {
        public AndSpecification(params Specification<T>[] specifications)
            : base(specifications) {}

        public override bool IsSatisfiedBy(T obj) {
            return specifications.All(specification => specification.IsSatisfiedBy(obj));
        }
    }

    public class OrSpecification<T> : CompositeSpecification<T> {
        public OrSpecification(params Specification<T>[] specifications) : base(specifications) {}

        public override bool IsSatisfiedBy(T obj) {
            return specifications.Any(specification => specification.IsSatisfiedBy(obj));
        }
    }

    public abstract class Specification<T> {
        public static Specification<T> Null = new NullSpecification<T>();
        public abstract bool IsSatisfiedBy(T obj);

        public static Specification<T> operator |(Specification<T> left, Specification<T> right) {
            return new OrSpecification<T>(left, right);
        }

        public static Specification<T> operator &(Specification<T> left, Specification<T> right) {
            return new AndSpecification<T>(left, right);
        }

        public static implicit operator Predicate<T>(Specification<T> specification) {
            return specification.IsSatisfiedBy;
        }

        public TSpecification ExtractSupersetSpecification<TSpecification>() {
            return (TSpecification) (object) ExtractSupersetSpecification(typeof (TSpecification));
        }

        private Specification<T> ExtractSupersetSpecification(Type specificationType) {
            if (GetType() == specificationType) {
                return this;
            }
            if (this is AndSpecification<T>) {
                return ((AndSpecification<T>) this).Specifications
                                                   .Select(specification => specification.ExtractSupersetSpecification(specificationType))
                                                   .FirstOrDefault(superset => superset != null);
            }
            return null;
        }
    }

    public abstract class CompositeSpecification<T> : Specification<T> {
        protected readonly List<Specification<T>> specifications;

        protected CompositeSpecification(params Specification<T>[] specifications) {
            this.specifications = new List<Specification<T>>(specifications);
        }

        public ReadOnlyCollection<Specification<T>> Specifications {
            get { return specifications.AsReadOnly(); }
        }

        public CompositeSpecification<T> Add(Specification<T> specification) {
            specifications.Add(specification);
            return this;
        }

        public CompositeSpecification<T> Remove(Specification<T> specification) {
            specifications.Remove(specification);
            return this;
        }
    }
}