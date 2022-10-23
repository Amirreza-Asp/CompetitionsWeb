using Ardalis.GuardClauses;
using System.Text.RegularExpressions;

namespace Competitions.SharedKernel.ValueObjects.Guards
{
    public static class CheckStudentNumberExtension
    {
        public static string ChackStudentNumebr ( this IGuardClause guardClause , string value , string parameterName = "file name" )
        {
            if ( value == null )
                throw new ArgumentNullException($"{parameterName} must have a value");

            if ( value.Trim().Length == 0 )
                throw new ArgumentException($"{parameterName} must have a value");

            Regex pattern = new Regex("^[0-9]{10}$");
            if ( string.IsNullOrEmpty(value) )
                throw new ArgumentException($"Invalid {parameterName} entered");

            return value;
        }
    }
}
