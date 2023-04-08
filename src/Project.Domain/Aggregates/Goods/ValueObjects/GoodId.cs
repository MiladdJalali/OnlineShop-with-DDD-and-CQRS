using System;
using System.Collections.Generic;

namespace Project.Domain.Aggregates.Goods.ValueObjects
{
    public class GoodId : ValueObject
    {
        private GoodId()
        {
        }

        public Guid Value { get; private init; }

        public static GoodId Create(Guid value)
        {
            return new GoodId { Value = value };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}