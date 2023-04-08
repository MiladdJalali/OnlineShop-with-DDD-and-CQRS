using System.Collections.Generic;
using Project.Domain.Aggregates.Users.Rules;

namespace Project.Domain.Aggregates.Users.ValueObjects
{
    public class UserAddress : ValueObject
    {
        private UserAddress()
        {
        }

        public string Value { get; private init; }

        public static UserAddress Create(string value)
        {
            CheckRule(new UserAddressCannotBeEmptyRule(value));

            return new UserAddress { Value = value };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}