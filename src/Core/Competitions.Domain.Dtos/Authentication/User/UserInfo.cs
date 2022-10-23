using Newtonsoft.Json;

namespace Competitions.Domain.Dtos.Authentication.User
{
    public class UserInfo
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("Active")]
        public long Active { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("enname")]
        public string Enname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("enlastname")]
        public string Enlastname { get; set; }

        [JsonProperty("fathername")]
        public string Fathername { get; set; }

        [JsonProperty("enfathername")]
        public string Enfathername { get; set; }

        [JsonProperty("idmelli")]
        public string Idmelli { get; set; }

        [JsonProperty("idnumber")]
        public string Idnumber { get; set; }

        [JsonProperty("placebirth")]
        public string Placebirth { get; set; }

        [JsonProperty("grade")]
        public string Grade { get; set; }

        [JsonProperty("student_number")]
        public long StudentNumber { get; set; }

        [JsonProperty("marital")]
        public long Marital { get; set; }

        [JsonProperty("religion")]
        public string Religion { get; set; }

        [JsonProperty("faith")]
        public string Faith { get; set; }

        [JsonProperty("citizen")]
        public string Citizen { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("addresswork")]
        public string Addresswork { get; set; }

        [JsonProperty("phonework")]
        public string Phonework { get; set; }

        [JsonProperty("addresshome")]
        public string Addresshome { get; set; }

        [JsonProperty("phonehome")]
        public string Phonehome { get; set; }

        [JsonProperty("postalcode")]
        public string Postalcode { get; set; }

        [JsonProperty("university")]
        public string University { get; set; }

        [JsonProperty("major")]
        public string Major { get; set; }

        [JsonProperty("trend")]
        public string Trend { get; set; }

        [JsonProperty("section")]
        public long Section { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
