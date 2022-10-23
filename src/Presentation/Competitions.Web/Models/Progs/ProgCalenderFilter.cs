using Competitions.Common.Helpers;
using System.Globalization;

namespace Competitions.Web.Models.Calenders
{
    public class ProgCalenderFilter
    {
        public DateTime ProgDate { get; set; } = DateTime.Now;

        public String Dir { get; set; }

        public void RoundMeetingsDate ()
        {
            PersianCalendar pc = new PersianCalendar();
            if ( ProgDate == default )
                ProgDate = DateTime.Now;


            ProgDate = new DateTime(pc.GetYear(ProgDate) , pc.GetMonth(ProgDate) , 1);
            ProgDate = ProgDate.ToMiladi();
        }


        public void Move ()
        {
            if ( !String.IsNullOrEmpty(Dir) && Dir.ToLower().Equals("next") )
                Next();
            else if ( !String.IsNullOrEmpty(Dir) && Dir.ToLower().Equals("prev") )
                Prev();
        }

        private void Next ()
        {
            if ( String.IsNullOrEmpty(Dir) || !Dir.ToLower().Equals("next") )
                return;

            var pc = new PersianCalendar();
            ProgDate = pc.AddMonths(ProgDate , 1);
        }

        private void Prev ()
        {
            if ( String.IsNullOrEmpty(Dir) || !Dir.ToLower().Equals("prev") )
                return;

            var pc = new PersianCalendar();
            ProgDate = pc.AddMonths(ProgDate , -1);
        }
    }
}
