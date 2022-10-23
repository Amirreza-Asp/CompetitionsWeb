namespace Competitions.Domain.Dtos.Places.Places
{
    public class GetPlaceDto
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Address { get; set; }
        public String Image { get; set; }
        public String SupervisorName { get; set; }
    }
}
