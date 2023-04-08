using System;
using Project.Domain.Aggregates.Goods.Events;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.ValueObjects;

namespace Project.Domain.Aggregates.Goods
{
    public class Good : Entity, IAggregateRoot
    {
        private Good()
        {
        }

        public GoodId Id { get; private set; }


        public GoodName Name { get; private set; }

        public GoodPrice Price { get; private set; }

        public GoodDiscount Discount { get; private set; }

        public GoodIsFragile IsFragile { get; private set; }

        public Description Description { get; private set; }

        public static Good Create(
            GoodId id,
            GoodName name,
            GoodPrice price,
            GoodDiscount discount,
            GoodIsFragile isFragile,
            Guid creatorId)
        {
            var good = new Good { Id = id };

            good.AddEvent(new GoodCreatedEvent(good.Id));
            good.TrackCreate(creatorId);

            good.ChangeName(name, creatorId);
            good.ChangePrice(price, creatorId);
            good.ChangeDiscount(discount, creatorId);
            good.ChangeIsFragile(isFragile, creatorId);

            return good;
        }

        public void ChangeName(GoodName name, Guid updaterId)
        {
            if (Name == name)
                return;

            AddEvent(new GoodNameChangedEvent(Id, Name, name));
            TrackUpdate(updaterId);

            Name = name;
        }

        public void ChangePrice(GoodPrice price, Guid updaterId)
        {
            if (Price == price)
                return;

            AddEvent(new GoodPriceChangedEvent(Id, Price, price));
            TrackUpdate(updaterId);

            Price = price;
        }

        public void ChangeDiscount(GoodDiscount discount, Guid updaterId)
        {
            if (Discount == discount)
                return;

            AddEvent(new GoodDiscountChangedEvent(Id, Discount, discount));
            TrackUpdate(updaterId);

            Discount = discount;
        }

        public void ChangeIsFragile(GoodIsFragile isFragile, Guid updaterId)
        {
            if (IsFragile == isFragile)
                return;

            AddEvent(new GoodIsFragileChangedEvent(Id, IsFragile, isFragile));
            TrackUpdate(updaterId);

            IsFragile = isFragile;
        }

        public void ChangeDescription(Description description, Guid updaterId)
        {
            if (Description == description)
                return;

            AddEvent(new GoodDescriptionChangedEvent(Id, Description, description));
            TrackUpdate(updaterId);

            Description = description;
        }

        public void Delete()
        {
            if (CanBeDeleted())
                throw new InvalidOperationException();

            AddEvent(new GoodDeletedEvent(Id));
            MarkAsDeleted();
        }
    }
}