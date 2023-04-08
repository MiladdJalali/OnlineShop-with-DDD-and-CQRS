using Project.Domain.Properties;

namespace Project.Domain.Aggregates.Users.Rules
{
    public class UserAddressCannotBeEmptyRule : IBusinessRule
    {
        private readonly string value;

        internal UserAddressCannotBeEmptyRule(string value)
        {
            this.value = value;
        }

        public string Message { get; } = DomainResources.User_AddressCannotBeEmpty;

        public string Details { get; } = string.Empty;

        public bool IsBroken()
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}