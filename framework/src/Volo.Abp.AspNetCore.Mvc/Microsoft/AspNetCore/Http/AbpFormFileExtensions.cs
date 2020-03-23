using System.IO;
using System.Threading.Tasks;

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

        public static async Task<byte[]> GetAllBytesAsync(this IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                return await stream.GetAllBytesAsync();
            }
        }
    }
}
