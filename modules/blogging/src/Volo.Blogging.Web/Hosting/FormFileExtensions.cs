using System.IO;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Volo.Abp;

namespace Volo.Blogging.Hosting
{
    public static class FormFileExtensions
    {
        public static byte[] AsBytes(this IFormFile file) //TODO: Move to the framework (rename to GetBytes)
        {
            using (var stream = file.OpenReadStream())
            {
                return stream.GetAllBytes();
            }
        }

        public static void ValidateImage([CanBeNull] this IFormFile file)
        {
            
        }
    }
}