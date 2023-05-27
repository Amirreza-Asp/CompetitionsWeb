using Ardalis.GuardClauses;

namespace Competitions.SharedKernel.ValueObjects.Guards
{
    public static class PhoneNumberGuradExtensions
    {
        public static string InvalidPhoneNumber(this IGuardClause guardClause, string value, string parameterName)
        {
            //if ( value == null )
            //    throw new ArgumentNullException($"{parameterName} must have a value");

            //if ( value.Trim().Length == 0 )
            //    throw new ArgumentException($"{parameterName} must have a value");

            return value;
        }
    }
}
