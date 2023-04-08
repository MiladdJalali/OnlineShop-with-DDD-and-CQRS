using System.Collections.Generic;
using Project.Domain.Aggregates.Goods.Rules;

namespace Project.Domain.Aggregates.Goods.ValueObjects
{
    public class GoodPrice : ValueObject
    {
        private GoodPrice()
        {
        }

        public decimal Value { get; private init; }

        public static GoodPrice Create(decimal value)
        {
            CheckRule(new GoodPriceMustBeValidRule(value));

            return new GoodPrice { Value = value };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}