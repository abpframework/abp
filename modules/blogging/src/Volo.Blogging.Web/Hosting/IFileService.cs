using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Volo.Blogging.Hosting
{
    public interface IFileService
    {
        string FileUploadDirectory { get; }

        string GenerateUniqueFileName(string extension, string prefix = null, string postfix = null);

        Task<string> SaveFormFileAndGetUrlAsync(IFormFile file);

        Task<string> SaveFileAsync(byte[] fileBytes, string originalFileName);
    }
}