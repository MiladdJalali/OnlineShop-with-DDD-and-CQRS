using System.Collections.Generic;
using Project.Domain.Aggregates.Goods.Rules;

namespace Project.Domain.Aggregates.Goods.ValueObjects
{
    public class GoodName : ValueObject
    {
        private GoodName()
        {
        }

        public string Value { get; private init; }

        public static GoodName Create(string value)
        {
            CheckRule(new GoodNameCannotBeEmptyRule(value));

            return new GoodName { Value = value };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}