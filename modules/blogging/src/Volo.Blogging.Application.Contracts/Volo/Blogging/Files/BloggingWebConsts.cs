using System;

namespace Volo.Blogging
{
    public class BloggingWebConsts
    {
        public class FileUploading
        {
            public const int MaxFileSize = 5242880; //5MB

            public static int MaxFileSizeAsMegabytes => Convert.ToInt32((MaxFileSize / 1024f) / 1024f);
        }
    }
}