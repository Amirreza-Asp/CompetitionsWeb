using Competitions.Application.Authentication.Interfaces;
using Competitions.Common;
using Competitions.Domain.Dtos.Authentication.Positions;
using Newtonsoft.Json;

namespace Competitions.Persistence.Authentication.Services
{
    public class PositionAPI : IPositionAPI
    {
        public async Task<IEnumerable<Position>> GetPositionsAsync ()
        {
            String url = $"https://khedmat.razi.ac.ir/api/KhedmatAPI/khedmat/users?action=getAllPositions&username=${SD.KhedmatRaziUserName}&password={SD.KhedmatRaziPassword}";
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
                var positions = JsonConvert.DeserializeObject<IEnumerable<Position>>(strContent);
                return positions;
            }

            return new List<Position>();
        }

        public async Task<IEnumerable<UserByPosition>> GetUsersAsync ( String position )
        {
            String url = $"https://khedmat.razi.ac.ir/api/KhedmatAPI/khedmat/users?action=usersWithJobPosition&username={SD.KhedmatRaziUserName}&password={SD.KhedmatRaziPassword}&position={position}";
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
                var user = JsonConvert.DeserializeObject<IEnumerable<UserByPosition>>(strContent);
                return user;
            }

            return new List<UserByPosition>();
        }
    }
}
