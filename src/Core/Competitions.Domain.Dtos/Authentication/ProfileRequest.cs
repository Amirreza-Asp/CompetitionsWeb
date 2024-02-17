namespace Competitions.Domain.Dtos.Authentication
{
    public class OAuthResponseToken
    {
        public string access_token { get; set; }
        public string id_token { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
        public object refresh_token { get; set; }
        public string token_type { get; set; }
    }
    public class ProfileRequest
    {
        public SSOUserInfo data { get; set; }
        public bool isSuccess { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
    }

    public class SSOUserInfo
    {
        public string id { get; set; }
        public string nationalId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fatherName { get; set; }
        public string mobile { get; set; }
        public string gender { get; set; }
        public string birthDate { get; set; }
        public string birthDateShamsi { get; set; }
        public string shenasnamehNo { get; set; }
        public string postalCode { get; set; }
        public string province { get; set; }
        public string city { get; set; }
    }
}
