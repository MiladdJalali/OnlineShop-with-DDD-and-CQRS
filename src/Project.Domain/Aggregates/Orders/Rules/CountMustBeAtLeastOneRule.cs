using Project.Domain.Properties;

namespace Project.Domain.Aggregates.Orders.Rules
{
    internal class CountMustBeAtLeastOneRule : IBusinessRule
    {
        private readonly decimal value;

        internal CountMustBeAtLeastOneRule(int value)
        {
            this.value = value;
        }

        public string Message { get; } = DomainResources.Order_CountMustBeAtleastOne;

        public string Details { get; } = string.Empty;

        public bool IsBroken()
        {
            return value < 1;
        }
    }
}