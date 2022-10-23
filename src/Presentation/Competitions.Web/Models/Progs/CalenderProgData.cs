namespace Competitions.Web.Models.Progs
{
    public class CalenderProgData
    {
        public CalenderProgData ( string name , DateTime start , string url )
        {
            Name = name;
            Start = start;
            Url = url;
        }

        public String Name { get; set; }
        public DateTime Start { get; set; }
        public String Url { get; set; }
    }
}
