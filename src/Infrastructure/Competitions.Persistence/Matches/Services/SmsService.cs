using Competitions.Application.Managment.Interfaces;
using Competitions.Common;

namespace Competitions.Persistence.Managment.Services
{
    public class SmsService : ISmsService
    {
        public async Task<bool> SendAsync ( String message , String phoneNumber )
        {
            String url = $"https://khedmat.razi.ac.ir/api/KhedmatAPI/message?action=sendSMS&username={SD.KhedmatRaziUserName}&password={SD.KhedmatRaziPassword}&text="
                + message + "&FromOutside=true&MobileNumber={\"MobileNumber\":[\"" + phoneNumber + "\"]}";
            var request = new HttpRequestMessage(HttpMethod.Post , url);
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var client = new HttpClient(handler);

            client.DefaultRequestVersion = new Version("1.1");


            var res = await client.SendAsync(request);

            return res.IsSuccessStatusCode;
        }
    }
}
