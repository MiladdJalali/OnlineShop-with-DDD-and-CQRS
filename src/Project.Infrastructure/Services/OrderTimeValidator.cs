using System;
using Project.Domain.Aggregates.Orders.Services;

namespace Project.Infrastructure.Services
{
    public class OrderTimeValidator : IOrderTimeValidator
    {
        public bool IsValid(DateTimeOffset orderTime)
        {
            var start = new TimeSpan(08, 0, 0);
            var end = new TimeSpan(19, 0, 0);

            var now = orderTime.TimeOfDay;

            return now > start || now < end;
        }
    }
}