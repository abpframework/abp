using System;

namespace Volo.CmsKit.Pages
{
    public class PageConsts
    {
        public const string EntityType = "Page";
        
        public static int MaxTitleLength { get; set; } = 256;
        
        public static int MaxSlugLength { get; set; } = 256;
        
        public static int MaxContentLength { get; set; } = int.MaxValue;
        
        public static int MaxScriptLength { get; set; } = int.MaxValue;
        
        public static int MaxStyleLength { get; set; } = int.MaxValue;
        
        private static string _urlPrefix = "/pages/";
        public static string UrlPrefix
        {
            get => _urlPrefix;
            set => _urlPrefix = value.EnsureEndsWith('/').EnsureStartsWith('/');
        }
    }
}