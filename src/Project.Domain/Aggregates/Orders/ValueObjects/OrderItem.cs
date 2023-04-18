using System.Collections.Generic;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.Aggregates.Orders.Rules;

namespace Project.Domain.Aggregates.Orders.ValueObjects
{
    public class OrderItem : ValueObject
    {
        private OrderItem()
        {
        }

        public GoodId GoodId { get; private init; }

        public int Count { get; private init; }

        public static OrderItem Create(GoodId goodId, int count)
        {
            CheckRule(new CountMustBeAtLeastOneRule(count));

            return new OrderItem
            {
                GoodId = goodId,
                Count = count
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return GoodId;
            yield return Count;
        }
    }
}