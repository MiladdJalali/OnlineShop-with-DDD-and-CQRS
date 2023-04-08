using System;
using System.Collections.Generic;
using Project.Domain.Aggregates.Orders.Services;
using Project.Domain.Properties;

namespace Project.Domain.Aggregates.Orders.Rules
{
    internal class ItemsTotalPriceMustBeValidRule : IBusinessRule
    {
        private readonly IEnumerable<Guid> goodIds;

        private readonly IGoodsTotalPriceValidator validator;

        internal ItemsTotalPriceMustBeValidRule(IEnumerable<Guid> goodIds, IGoodsTotalPriceValidator validator)
        {
            this.goodIds = goodIds;
            this.validator = validator;
        }

        public string Message { get; } = DomainResources.Order_ItemTotalPriceMustBeValid;

        public string Details { get; } = string.Empty;

        public bool IsBroken()
        {
            return !validator.IsTotalPriceValid(goodIds).GetAwaiter().GetResult();
        }
    }
}