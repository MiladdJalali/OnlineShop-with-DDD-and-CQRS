using System.Collections.Generic;
using Project.Domain.Aggregates.Users.Rules;

namespace Project.Domain.Aggregates.Users.ValueObjects
{
    public class UserUsername : ValueObject
    {
        private UserUsername()
        {
        }

        public string Value { get; private init; }

        public static UserUsername Create(string value)
        {
            CheckRule(new UserUsernameCannotBeEmptyRule(value));

            return new UserUsername { Value = value };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}