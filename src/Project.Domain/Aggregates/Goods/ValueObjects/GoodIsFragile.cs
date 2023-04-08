using System.Collections.Generic;

namespace Project.Domain.Aggregates.Goods.ValueObjects
{
    public class GoodIsFragile : ValueObject
    {
        private GoodIsFragile()
        {
        }

        public bool Value { get; private init; }

        public static GoodIsFragile Create(bool value)
        {
            return new GoodIsFragile { Value = value };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}