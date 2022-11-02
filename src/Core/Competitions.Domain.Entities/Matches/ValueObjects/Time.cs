namespace Competitions.Domain.Entities.Managment.ValueObjects
{
    public class Time : ValueObject<Time>
    {
        public Time ( int hour , int min )
        {
            Hour = GuardAgaintWrongHour(hour);
            Min = GuardAgaintWrongMin(min);
        }

        public int Hour { get; }
        public int Min { get; }

        public static implicit operator string ( Time time ) => time.Hour.ToString("00") + ":" + time.Min.ToString("00");
        public static implicit operator Time ( String time )
        {
            var paras = time.Split(':');
            try
            {
                var hour = int.Parse(paras[0]);
                var min = int.Parse(paras[1]);

                return new Time(hour , min);
            }
            catch ( Exception ex )
            {
                throw new Exception($"The entered time {time} is wrong");
            }
        }

        public int GuardAgaintWrongHour ( int hour )
        {
            if ( hour < 0 && hour > 23 )
            {
                throw new ArgumentException("Hour must between 0 to 23");
            }

            return hour;
        }

        public int GuardAgaintWrongMin ( int min )
        {
            if ( min < 0 && min > 59 )
            {
                throw new ArgumentException("Hour must between 0 to 59");
            }

            return min;
        }

        protected override bool EqualsCore ( Time other )
        {
            return Hour.Equals(other.Hour) && Min.Equals(other.Min);
        }
    }
}
