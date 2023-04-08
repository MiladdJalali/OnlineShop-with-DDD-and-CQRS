using System;
using System.Collections.Generic;

namespace Project.Domain.Aggregates.Users.ValueObjects
{
    public class UserId : ValueObject
    {
        private UserId()
        {
        }

        public Guid Value { get; private init; }

        public static UserId Create(Guid value)
        {
            return new UserId { Value = value };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}