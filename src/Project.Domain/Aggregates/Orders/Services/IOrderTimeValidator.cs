using System;

namespace Project.Domain.Aggregates.Orders.Services
{
    public interface IOrderTimeValidator
    {
        bool IsValid(DateTimeOffset orderTime);
    }
}