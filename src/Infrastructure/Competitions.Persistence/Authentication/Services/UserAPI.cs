using Competitions.Application.Authentication.Interfaces;
using Competitions.Domain.Dtos.Authentication.User;
using Newtonsoft.Json;

namespace Competitions.Persistence.Authentication.Services
{
    public class UserAPI : IUserAPI
    {

        public async Task<UserInfo> GetUserAsync ( String nationalCode )
        {
            String url = $"https://khedmat.razi.ac.ir/api/KhedmatAPI/khedmat/users?action=details&username=GXBsBt9n&password=qwe159asd753&nationalCode={nationalCode}";
            var request = new HttpRequestMessage(HttpMethod.Post , url);
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var client = new HttpClient(handler);

            client.DefaultRequestVersion = new Version("1.1");



            var res = await client.SendAsync(request);

            if ( res.IsSuccessStatusCode )
            {
                var strContent = await res.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserInfo>(strContent);
                return user;
            }

            return null;
        }


    }
}
