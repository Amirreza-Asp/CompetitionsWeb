namespace Competitions.Domain.Entities.Managment.ValueObjects
{
    public class DateTimeRange : ValueObject<DateTimeRange>
    {
        public DateTimeRange ( DateTime from , DateTime to )
        {
            From = Guard.Against.Default(from);
            To = Guard.Against.Default(to);

            if ( To.Date < From.Date )
                throw new ArgumentException("To Must be greater than From");
        }

        public DateTime From { get; }
        public DateTime To { get; }

        protected override bool EqualsCore ( DateTimeRange other )
        {
            return From.Equals(other.From) && To.Equals(other.To);
        }
    }
}
