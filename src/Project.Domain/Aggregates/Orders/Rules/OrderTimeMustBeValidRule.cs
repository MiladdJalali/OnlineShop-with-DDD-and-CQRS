using System;
using Project.Domain.Aggregates.Orders.Services;
using Project.Domain.Properties;

namespace Project.Domain.Aggregates.Orders.Rules
{
    internal class OrderTimeMustBeValidRule : IBusinessRule
    {
        private readonly DateTimeOffset orderTime;
        private readonly IOrderTimeValidator orderTimeValidator;

        internal OrderTimeMustBeValidRule(DateTimeOffset orderTime, IOrderTimeValidator orderTimeValidator)
        {
            this.orderTimeValidator = orderTimeValidator;
            this.orderTime = orderTime;
        }

        public string Message { get; } = DomainResources.Order_OrderTimeIsInvalid;

        public string Details { get; } = string.Empty;

        public bool IsBroken()
        {
            return !orderTimeValidator.IsValid(orderTime);
        }
    }
}