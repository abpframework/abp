﻿namespace Volo.CmsKit.Pages
{
    public class PageConsts
    {
        public const string EntityType = "Page";

        public static int MaxTitleLength { get; set; } = 256;

        public static int MaxSlugLength { get; set; } = 256;
        
        public static int MaxContentLength { get; set; } = int.MaxValue;
    }
}