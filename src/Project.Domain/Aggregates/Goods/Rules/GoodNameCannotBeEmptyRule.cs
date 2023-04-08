using Project.Domain.Properties;

namespace Project.Domain.Aggregates.Goods.Rules
{
    public class GoodNameCannotBeEmptyRule : IBusinessRule
    {
        private readonly string value;

        internal GoodNameCannotBeEmptyRule(string value)
        {
            this.value = value;
        }

        public string Message { get; } = DomainResources.Good_GoodNameCannotBeEmpty;

        public string Details { get; } = string.Empty;

        public bool IsBroken()
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}