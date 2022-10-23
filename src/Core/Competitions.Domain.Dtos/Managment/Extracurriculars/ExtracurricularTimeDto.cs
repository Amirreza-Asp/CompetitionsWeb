namespace Competitions.Domain.Dtos.Managment.Extracurriculars
{
    public class ExtracurricularTimeDto
    {
        public String Day { get; set; }
        public String Time { get; set; }


        public int GetMin () => int.Parse(Time.Split(':')[1]);
        public int GetHour () => int.Parse(Time.Split(':')[0]);
    }
}
