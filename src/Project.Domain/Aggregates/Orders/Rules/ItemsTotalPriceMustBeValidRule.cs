using Project.Domain.Aggregates.Orders.Services;
using Project.Domain.Aggregates.Orders.ValueObjects;
using Project.Domain.Properties;

namespace Project.Domain.Aggregates.Orders.Rules
{
    internal class ItemsTotalPriceMustBeValidRule : IBusinessRule
    {
        private readonly OrderItem[] items;

        private readonly IGoodsTotalPriceValidator validator;

        internal ItemsTotalPriceMustBeValidRule(OrderItem[] items, IGoodsTotalPriceValidator validator)
        {
            this.items = items;
            this.validator = validator;
        }

        public string Message { get; } = DomainResources.Order_ItemTotalPriceMustBeValid;

        public string Details { get; } = string.Empty;

        public bool IsBroken()
        {
            return !validator.IsTotalPriceValid(items).GetAwaiter().GetResult();
        }
    }
}