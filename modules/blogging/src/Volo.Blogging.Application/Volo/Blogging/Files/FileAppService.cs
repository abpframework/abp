using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Validation;
using Volo.Blogging.Areas.Blog.Helpers;

namespace Volo.Blogging.Files
{
    public class FileAppService : BloggingAppServiceBase, IFileAppService
    {
        protected IBlobContainer<BloggingFileContainer> BlobContainer { get; }

        public FileAppService(
            IBlobContainer<BloggingFileContainer> blobContainer)
        {
            BlobContainer = blobContainer;
        }

        public virtual async Task<RawFileDto> GetAsync(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return new RawFileDto
            {
                Bytes = await BlobContainer.GetAllBytesAsync(name)
            };
        }

        public virtual async Task<FileUploadOutputDto> CreateAsync(FileUploadInputDto input)
        {
            if (input.Bytes.IsNullOrEmpty())
            {
                ThrowValidationException("Bytes of file can not be null or empty!", "Bytes");
            }

            if (input.Bytes.Length > BloggingWebConsts.FileUploading.MaxFileSize)
            {
                throw new UserFriendlyException($"File exceeds the maximum upload size ({BloggingWebConsts.FileUploading.MaxFileSizeAsMegabytes} MB)!");
            }

            if (!ImageFormatHelper.IsValidImage(input.Bytes, FileUploadConsts.AllowedImageUploadFormats))
            {
                throw new UserFriendlyException("Invalid image format!");
            }

            var uniqueFileName = GenerateUniqueFileName(Path.GetExtension(input.Name));

            await BlobContainer.SaveAsync(uniqueFileName, input.Bytes);

            return new FileUploadOutputDto
            {
                Name = uniqueFileName,
                WebUrl = "/api/blogging/files/www/" + uniqueFileName
            };
        }

        private static void ThrowValidationException(string message, string memberName)
        {
            throw new AbpValidationException(message,
                new List<ValidationResult>
                {
                    new ValidationResult(message, new[] {memberName})
                });
        }

        protected virtual string GenerateUniqueFileName(string extension, string prefix = null, string postfix = null)
        {
            return prefix + GuidGenerator.Create().ToString("N") + postfix + extension;
        }
    }
}
