namespace Competitions.Web.Models.Matches
{
    public class CalenderMatchData
    {
        public CalenderMatchData ( string name , DateTime start , DateTime end , string url , bool gender )
        {
            Name = name;
            Start = start;
            End = end;
            Url = url;
            Gender = gender;
        }

        public String Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public String Url { get; set; }
        public bool Gender { get; set; }
    }
}
