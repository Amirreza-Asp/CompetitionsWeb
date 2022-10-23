using Ardalis.GuardClauses;
using Competitions.SharedKernel.ValueObjects.Exceptions;
using System.Text.RegularExpressions;

namespace Competitions.SharedKernel.ValueObjects.Guards
{
    public static class PhoneNumberGuradExtensions
    {
        public static string InvalidPhoneNumber ( this IGuardClause guardClause , string value , string parameterName )
        {
            if ( value == null )
                throw new ArgumentNullException($"{parameterName} must have a value");

            if ( value.Trim().Length == 0 )
                throw new ArgumentException($"{parameterName} must have a value");

            Regex regex = new Regex("(0|\\+98)?([ ]|-|[()]){0,2}9[1|2|3|4|5|6|7|8|9]([ ]|-|[()]){0,2}(?:[0-9]([ ]|-|[()]){0,2}){8}");
            if ( !regex.IsMatch(value) )
                throw new InvalidPhoneNumberException($"The entered {parameterName} its wrong");

            return value;
        }
    }
}
