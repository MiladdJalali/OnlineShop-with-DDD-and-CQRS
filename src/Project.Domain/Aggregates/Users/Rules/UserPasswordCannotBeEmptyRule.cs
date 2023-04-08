using Project.Domain.Properties;

namespace Project.Domain.Aggregates.Users.Rules
{
    internal class UserPasswordCannotBeEmptyRule : IBusinessRule
    {
        private readonly string value;

        internal UserPasswordCannotBeEmptyRule(string value)
        {
            this.value = value;
        }

        public string Message { get; } = DomainResources.User_PasswordCannotBeEmpty;

        public string Details { get; } = string.Empty;

        public bool IsBroken()
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}