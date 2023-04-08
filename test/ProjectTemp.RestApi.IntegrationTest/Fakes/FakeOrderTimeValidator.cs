using System;
using Project.Domain.Aggregates.Orders.Services;

namespace Project.RestApi.IntegrationTest.Fakes
{
    public class FakeOrderTimeValidator : IOrderTimeValidator
    {
        public bool IsValid(DateTimeOffset orderTime)
        {
            return true;
        }
    }
}