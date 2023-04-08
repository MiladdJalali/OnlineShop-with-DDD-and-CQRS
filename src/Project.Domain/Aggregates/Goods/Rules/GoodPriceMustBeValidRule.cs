using Project.Domain.Properties;

namespace Project.Domain.Aggregates.Goods.Rules
{
    internal class GoodPriceMustBeValidRule : IBusinessRule
    {
        private readonly decimal value;

        internal GoodPriceMustBeValidRule(decimal value)
        {
            this.value = value;
        }

        public string Message { get; } = DomainResources.Good_GoodPriceMustBeValidRule;

        public string Details { get; } = string.Empty;

        public bool IsBroken()
        {
            return value < 0;
        }
    }
}