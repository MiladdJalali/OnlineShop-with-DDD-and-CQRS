using System.Collections.Generic;
using Project.Domain.Aggregates.Users.Rules;

namespace Project.Domain.Aggregates.Users.ValueObjects
{
    public class UserPassword : ValueObject
    {
        private UserPassword()
        {
        }

        public string Value { get; private init; }

        public static UserPassword Create(string value)
        {
            CheckRule(new UserPasswordCannotBeEmptyRule(value));

            return new UserPassword { Value = value };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}