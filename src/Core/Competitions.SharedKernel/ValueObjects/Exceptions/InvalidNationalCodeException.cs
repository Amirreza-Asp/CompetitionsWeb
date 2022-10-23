namespace Competitions.SharedKernel.ValueObjects.Exceptions
{
    public class InvalidNationalCodeException : Exception
    {
        public InvalidNationalCodeException ( string? message ) : base(message)
        {
        }
    }
}
