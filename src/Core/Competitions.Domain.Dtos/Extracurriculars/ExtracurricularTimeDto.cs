namespace Competitions.Domain.Dtos.Extracurriculars
{
    public class ExtracurricularTimeDto
    {
        public string Day { get; set; }
        public string Time { get; set; }


        public int GetMin () => int.Parse(Time.Split(':')[1]);
        public int GetHour () => int.Parse(Time.Split(':')[0]);
    }
}
