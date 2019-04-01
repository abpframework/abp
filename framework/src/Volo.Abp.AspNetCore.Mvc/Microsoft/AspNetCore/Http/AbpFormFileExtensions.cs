using System.IO;

namespace Microsoft.AspNetCore.Http
{
    public static class AbpFormFileExtensions
    {
        public static byte[] GetAllBytes(this IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                return stream.GetAllBytes();
            }
        }
    }
}
