namespace Competitions.Domain.Dtos.Authentication.User
{
    public class UserKhedmat
    {
        public class UserKhedmatTeacher
        {
            public string name { get; set; }
            public string last_name { get; set; }
            public string username { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string national_code { get; set; }
            public string phone_home { get; set; }
            public string student_number { get; set; }
            public string sex { get; set; }
            public string marry_status { get; set; }
            public string domestic { get; set; }
            public string grade { get; set; }
            public int active { get; set; }
            public string image { get; set; }
            public string faculty_id { get; set; }
            public string group_id { get; set; }
            public string major_id { get; set; }
            public string faculty_name { get; set; }
            public string group_name { get; set; }
            public string major_name { get; set; }

        }

        public class UserKhedmatStudent
        {
            public string name { get; set; }
            public string last_name { get; set; }
            public string username { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public string national_code { get; set; }
            public string phone_home { get; set; }
            public int student_number { get; set; }
            public int sex { get; set; }
            public int marry_status { get; set; }
            public string domestic { get; set; }
            public string grade { get; set; }
            public int active { get; set; }
            public string image { get; set; }
            public string faculty_id { get; set; }
            public string group_id { get; set; }
            public string major_id { get; set; }
            public string faculty_name { get; set; }
            public string group_name { get; set; }
            public string major_name { get; set; }

        }

        public class KhedmatResponseStudent
        {
            public string status { get; set; }
            public UserKhedmatStudent userInfo { get; set; }
        }

        public class KhedmatResponseTeacher
        {
            public string status { get; set; }
            public UserKhedmatTeacher userInfo { get; set; }
        }
    }
}
