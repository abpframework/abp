using System.IO;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Volo.Abp;
using Volo.Blogging.Areas.Blog.Helpers;

namespace Volo.Blogging.Hosting
{
    public static class FormFileExtensions
    {
        public static byte[] AsBytes(this IFormFile file)
        {
            byte[] fileBytes;
            using (var stream = file.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
            }

            return fileBytes;
        }

        public static void ValidateImage([CanBeNull] this IFormFile file, out byte[] fileBytes)
        {
            fileBytes = null;

            if (file == null)
            {
                throw new UserFriendlyException("No file found!");
            }

            if (file.Length <= 0)
            {
                throw new UserFriendlyException("File is empty!");
            }

            if (!file.ContentType.Contains("image"))
            {
                throw new UserFriendlyException("Not a valid image!");
            }

            using (var stream = file.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
            }

            if (!ImageFormatHelper.IsValidImage(fileBytes, BloggingWebConsts.FileUploading.AllowedImageUploadFormats))
            {
                throw new UserFriendlyException("Not a valid image format!");
            }

            if (file.Length > BloggingWebConsts.FileUploading.MaxFileSize)
            {
                throw new UserFriendlyException($"File exceeds the maximum upload size ({BloggingWebConsts.FileUploading.MaxFileSizeAsMegabytes} MB)!");
            }
        }
    }
}