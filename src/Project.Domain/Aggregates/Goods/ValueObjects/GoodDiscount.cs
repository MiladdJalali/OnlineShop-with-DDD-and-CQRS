using System.Collections.Generic;
using Project.Domain.Aggregates.Goods.Rules;

namespace Project.Domain.Aggregates.Goods.ValueObjects
{
    public class GoodDiscount : ValueObject
    {
        private GoodDiscount()
        {
        }

        public decimal Value { get; private init; }

        public static GoodDiscount Create(decimal value)
        {
            CheckRule(new GoodDiscountMustBeValidRule(value));

            return new GoodDiscount { Value = value };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}