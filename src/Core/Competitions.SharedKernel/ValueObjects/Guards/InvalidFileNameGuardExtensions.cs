using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competitions.SharedKernel.ValueObjects.Guards
{
    public static class InvalidFileNameGuardExtensions
    {
        public static string InvalidFileName(this IGuardClause guardClause, string value, string parameterName = "file name")
        {
            if (value == null)
                throw new ArgumentNullException($"{parameterName} must have a value");

            if (value.Trim().Length == 0)
                throw new ArgumentException($"{parameterName} must have a value");

            string extension = Path.GetExtension(value);
            if (string.IsNullOrEmpty(extension))
                throw new ArgumentException($"Invalid {parameterName} entered");

            return value;
        }
    }
}
