using Competitions.Domain.Entities.Managment.ValueObjects;

namespace Competitions.Domain.Entities.Managment
{
    public class ExtracurricularTime : BaseEntity<long>
    {
        public ExtracurricularTime ( string day , Time time , Guid extracurricularId )
        {
            Day = Guard.Against.NullOrEmpty(day);
            Time = time;
            ExtracurricularId = Guard.Against.Default(extracurricularId);
        }

        private ExtracurricularTime ()
        {
        }

        public String Day { get; private set; }
        public Time Time { get; private set; }
        public Guid ExtracurricularId { get; private set; }


        public String ToJson () => $"{{ day : \" {Day} \" , time : \"  {Time.Hour.ToString("00")}:{Time.Min.ToString("00")} \" }}";

        public Extracurricular Extracurricular { get; private set; }

        public bool Equal ( String day , String time )
        {
            return day == Day && time == Time;
        }
    }
}
