using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Blogging.Hosting
{
    public class FileService : IFileService, ITransientDependency
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IGuidGenerator _guidGenerator;

        public FileService(IHostingEnvironment hostingEnvironment, IGuidGenerator guidGenerator)
        {
            _hostingEnvironment = hostingEnvironment;
            _guidGenerator = guidGenerator;
        }

        public string FileUploadDirectory
        {
            get
            {
                var uploadDirectory = Path.Combine(_hostingEnvironment.WebRootPath, BloggingWebConsts.FileUploading.DefaultFileUploadFolderName);
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                return uploadDirectory;
            }
        }

        public string GenerateUniqueFileName(string extension, string prefix = null, string postfix = null)
        {
            return prefix + _guidGenerator.Create().ToString("N") + postfix + extension;
        }

        public async Task<string> SaveFormFileAndGetUrlAsync(IFormFile file)
        {
            var uniqueFileName = await SaveFileInternalAsync(file.FileName, file.AsBytes());
            return GetFileUrl(uniqueFileName);
        }

        public async Task<string> SaveFileAsync(byte[] fileBytes, string originalFileName)
        {
            if (fileBytes == null || fileBytes.Length == 0)
            {
                throw new UserFriendlyException("File is empty!");
            }

            var uniqueFileName =  await SaveFileInternalAsync(originalFileName, fileBytes);
            return GetFileUrl(uniqueFileName);
        }

        private static string GetFileUrl(string uniqueFileName)
        {
            return "/" + BloggingWebConsts.FileUploading.DefaultFileUploadFolderName + "/" + uniqueFileName;
        }

        private async Task<string> SaveFileInternalAsync(string originalFileName, byte[] fileBytes)
        {
            var uniqueFileName = GenerateUniqueFileName(Path.GetExtension(originalFileName));
            var filePath = Path.Combine(FileUploadDirectory, uniqueFileName);
            File.WriteAllBytes(filePath, fileBytes); //TODO: Previously was using WriteAllBytesAsync, but it's only in .netcore.
            return uniqueFileName;
        }

    }
}