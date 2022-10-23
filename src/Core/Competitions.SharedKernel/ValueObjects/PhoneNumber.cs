using Ardalis.GuardClauses;
using Competitions.SharedKernel.Common;
using Competitions.SharedKernel.ValueObjects.Guards;

namespace Competitions.SharedKernel.ValueObjects
{
    public class PhoneNumber : ValueObject<PhoneNumber>
    {
        public PhoneNumber ( string value )
        {
            Value = Guard.Against.InvalidPhoneNumber(value , nameof(PhoneNumber));
        }

        public string Value { get; }

        protected override bool EqualsCore ( PhoneNumber other )
        {
            return Value == other.Value;
        }

        public static implicit operator string ( PhoneNumber phoneNumber ) => phoneNumber.Value;
        public static implicit operator PhoneNumber ( string value ) => new PhoneNumber(value);
    }
}
