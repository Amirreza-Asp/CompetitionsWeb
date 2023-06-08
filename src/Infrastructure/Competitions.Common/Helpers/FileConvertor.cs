using Microsoft.AspNetCore.Http;

namespace Competitions.Common.Helpers
{
    public static class FileConvertor
    {
        public static byte[] ReadBytes(this IFormFile file)
        {
            byte[] obj = null;
            using (var fileStream = file.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    obj = memoryStream.ToArray();
                }
            }
            return obj;
        }

    }
}
