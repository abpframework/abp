using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Linq;

namespace Volo.Blogging
{
    public class BloggingWebConsts
    {
        public class FileUploading
        {
            public const string DefaultFileUploadFolderName = "files";

            public static readonly ICollection<ImageFormat> AllowedImageUploadFormats = new Collection<ImageFormat>
            {
                ImageFormat.Jpeg,
                ImageFormat.Png,
                ImageFormat.Gif,
                ImageFormat.Bmp
            };

            public static string AllowedImageFormatsJoint => string.Join(",", AllowedImageUploadFormats.Select(x => x.ToString()));

            public const int MaxFileSize = 5242880; //5MB

            public static int MaxFileSizeAsMegabytes => Convert.ToInt32((MaxFileSize / 1024f) / 1024f);
        }
    }
}