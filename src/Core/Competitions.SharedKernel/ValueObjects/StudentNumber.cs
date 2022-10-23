using Ardalis.GuardClauses;
using Competitions.SharedKernel.ValueObjects.Guards;

namespace Competitions.SharedKernel.ValueObjects
{
    public class StudentNumber
    {
        public StudentNumber ( string value )
        {
            Value = Guard.Against.ChackStudentNumebr(value);
        }

        private StudentNumber () { }

        public string Value { get; private set; }

        public static implicit operator string ( StudentNumber studentNumber ) => studentNumber.Value;
        public static implicit operator StudentNumber ( string value ) => new StudentNumber(value);
    }
}
