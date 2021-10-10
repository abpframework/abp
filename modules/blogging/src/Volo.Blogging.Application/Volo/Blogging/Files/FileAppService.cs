using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
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

        public virtual async Task<IRemoteStreamContent> GetFileAsync(string name)
        {
            var fileStream = await BlobContainer.GetAsync(name);
            return new RemoteStreamContent(fileStream, name, GetByExtension(Path.GetExtension(name)), disposeStream: true);
        }

        private static string GetByExtension(string extension)
        {
            extension = extension.RemovePreFix(".").ToLowerInvariant();

            switch (extension)
            {
                case "png":
                    return "image/png";
                case "gif":
                    return "image/gif";
                case "jpg":
                case "jpeg":
                    return "image/jpeg";

                //TODO: Add other extensions too..

                default:
                    return "application/octet-stream";
            }
        }

        public virtual async Task<FileUploadOutputDto> CreateAsync(FileUploadInputDto input)
        {
            if (input.File == null)
            {
                ThrowValidationException("Bytes of file can not be null or empty!", nameof(input.File));
            }

            if (input.File.ContentLength > BloggingWebConsts.FileUploading.MaxFileSize)
            {
                throw new UserFriendlyException($"File exceeds the maximum upload size ({BloggingWebConsts.FileUploading.MaxFileSizeAsMegabytes} MB)!");
            }

            var position = input.File.GetStream().Position;

            if (!ImageFormatHelper.IsValidImage(input.File.GetStream(), FileUploadConsts.AllowedImageUploadFormats))
            {
                throw new UserFriendlyException("Invalid image format!");
            }

            // IsValidImage may change the position of the stream
            if (input.File.GetStream().CanSeek)
            {
                input.File.GetStream().Position = position;
            }

            var uniqueFileName = GenerateUniqueFileName(Path.GetExtension(input.Name));

            await BlobContainer.SaveAsync(uniqueFileName, input.File.GetStream());

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
