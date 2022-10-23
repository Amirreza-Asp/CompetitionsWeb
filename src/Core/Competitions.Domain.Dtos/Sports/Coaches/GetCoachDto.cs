namespace Competitions.Domain.Dtos.Sports.Coaches
{
    public class GetCoachDto
    {
        public long Id { get; set; }
        public String FullName { get; set; }
        public String Sport { get; set; }
        public String PhoneNumber { get; set; }
        public String NationalCode { get; set; }
    }
}
