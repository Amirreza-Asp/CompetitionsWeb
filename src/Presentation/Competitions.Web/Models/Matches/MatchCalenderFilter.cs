using Competitions.Common.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Competitions.Web.Models.Calenders
{
    public class MatchCalenderFilter
    {
        public DateTime MatchDate { get; set; } = DateTime.Now;

        public String Dir { get; set; }

        public String? Level { get; set; } = "درون دانشگاهی";

        public void RoundMeetingsDate ()
        {
            PersianCalendar pc = new PersianCalendar();
            if ( MatchDate == default )
                MatchDate = DateTime.Now;


            MatchDate = new DateTime(pc.GetYear(MatchDate) , pc.GetMonth(MatchDate) , 1);
            MatchDate = MatchDate.ToMiladi();
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
            MatchDate = pc.AddMonths(MatchDate , 1);
        }

        private void Prev ()
        {
            if ( String.IsNullOrEmpty(Dir) || !Dir.ToLower().Equals("prev") )
                return;

            var pc = new PersianCalendar();
            MatchDate = pc.AddMonths(MatchDate , -1);
        }



        public IEnumerable<SelectListItem> GetLevels ()
        {
            return new List<SelectListItem>
            {
                new SelectListItem(){Text = "درون دانشگاهی" , Value = "درون دانشگاهی"},
                new SelectListItem(){Text = "استانی" , Value = "استانی"},
                new SelectListItem(){Text = "منطقه ای" , Value = "منطقه ای"},
                new SelectListItem(){Text = "المپیاد" , Value = "المپیاد"},
                new SelectListItem(){Text = "کشوری" , Value = "کشوری"},
                new SelectListItem(){Text = "بین مناطق" , Value = "بین مناطق"},
            };
        }


    }
}
