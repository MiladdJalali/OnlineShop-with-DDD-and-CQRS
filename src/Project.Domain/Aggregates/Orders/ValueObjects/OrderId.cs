using System;
using System.Collections.Generic;

namespace Project.Domain.Aggregates.Orders.ValueObjects
{
    public class OrderId : ValueObject
    {
        private OrderId()
        {
        }

        public Guid Value { get; private init; }

        public static OrderId Create(Guid value)
        {
            return new OrderId { Value = value };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}