using System;
using Project.Domain.Aggregates.Goods;
using Project.Domain.Aggregates.Goods.ValueObjects;

namespace Project.Domain.UnitTest.Aggregates.Goods.Builders
{
    public class GoodBuilder
    {
        private Guid creatorId;

        private decimal discount;
        private Guid id;

        private bool isFragile;

        private string name;

        private decimal price;

        public GoodBuilder()
        {
            id = Guid.NewGuid();
            name = "Name";
            price = 1000;
            discount = 10;
            isFragile = true;
        }

        public Good Build()
        {
            return Good.Create(
                GoodId.Create(id),
                GoodName.Create(name),
                GoodPrice.Create(price),
                GoodDiscount.Create(discount),
                GoodIsFragile.Create(isFragile),
                creatorId);
        }

        public GoodBuilder WithId(Guid value)
        {
            id = value;
            return this;
        }

        public GoodBuilder WithGoodName(string value)
        {
            name = value;
            return this;
        }

        public GoodBuilder WithPrice(decimal value)
        {
            price = value;
            return this;
        }

        public GoodBuilder WithDiscount(decimal value)
        {
            discount = value;
            return this;
        }

        public GoodBuilder WithIsFragile(bool value)
        {
            isFragile = value;
            return this;
        }

        public GoodBuilder WithCreatorId(Guid value)
        {
            creatorId = value;
            return this;
        }
    }
}