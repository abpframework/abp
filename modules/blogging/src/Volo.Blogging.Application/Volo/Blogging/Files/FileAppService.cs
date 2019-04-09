using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Validation;
using Volo.Blogging.Areas.Blog.Helpers;

namespace Volo.Blogging.Files
{
    public class FileAppService : ApplicationService, IFileAppService
    {
        public BlogFileOptions Options { get; }

        public FileAppService(IOptions<BlogFileOptions> options)
        {
            Options = options.Value;
        }

        public virtual Task<RawFileDto> GetAsync(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var filePath = Path.Combine(Options.FileUploadLocalFolder, name);

            return Task.FromResult(
                new RawFileDto
                {
                    Bytes = File.ReadAllBytes(filePath)
                }
            );
        }

        public virtual Task<FileUploadOutputDto> CreateAsync(FileUploadInputDto input)
        {
            if (input.Bytes.IsNullOrEmpty())
            {
                ThrowValidationException("Bytes can not be null or empty!", "Bytes");
            }

            if (input.Bytes.Length > BloggingWebConsts.FileUploading.MaxFileSize)
            {
                throw new UserFriendlyException($"File exceeds the maximum upload size ({BloggingWebConsts.FileUploading.MaxFileSizeAsMegabytes} MB)!");
            }

            if (!ImageFormatHelper.IsValidImage(input.Bytes, FileUploadConsts.AllowedImageUploadFormats))
            {
                throw new UserFriendlyException("Not a valid image format!");
            }

            var uniqueFileName = GenerateUniqueFileName(Path.GetExtension(input.Name));
            var filePath = Path.Combine(Options.FileUploadLocalFolder, uniqueFileName);

            File.WriteAllBytes(filePath, input.Bytes); //TODO: Previously was using WriteAllBytesAsync, but it's only in .netcore.

            return Task.FromResult(new FileUploadOutputDto
            {
                Name = uniqueFileName,
                WebUrl = "/api/blogging/files/www/" + uniqueFileName
            });
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
