using Ardalis.GuardClauses;
using Competitions.SharedKernel.ValueObjects.Exceptions;
using System.Text.RegularExpressions;

namespace Competitions.SharedKernel.ValueObjects.Guards
{
    public static class NationalCodeGuardExtensions
    {
        public static string InvalidNationalCode ( this IGuardClause guardClause , string value , string parameterName )
        {
            if ( value == null )
                throw new ArgumentNullException($"{parameterName} must have a value");

            if ( value.Trim().Length == 0 )
                throw new ArgumentException($"{parameterName} must have a value");

            Regex pattern = new Regex("^[0-9]{10}$");

            if ( pattern.IsMatch(value) )
                return value;

            throw new InvalidNationalCodeException($"{parameterName} is not a national code");
        }
    }
}
