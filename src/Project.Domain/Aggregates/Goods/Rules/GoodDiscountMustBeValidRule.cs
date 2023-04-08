using Project.Domain.Properties;

namespace Project.Domain.Aggregates.Goods.Rules
{
    internal class GoodDiscountMustBeValidRule : IBusinessRule
    {
        private readonly decimal value;

        internal GoodDiscountMustBeValidRule(decimal value)
        {
            this.value = value;
        }

        public string Message { get; } = DomainResources.Good_GoodDiscountMustBeValidRule;

        public string Details { get; } = string.Empty;

        public bool IsBroken()
        {
            return value is < 0 or > 100;
        }
    }
}