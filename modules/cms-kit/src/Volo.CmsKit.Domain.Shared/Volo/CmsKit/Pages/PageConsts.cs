using System;

namespace Volo.CmsKit.Pages;

public static class PageConsts
{
    public const string EntityType = "Page";

    public static int MaxTitleLength { get; set; } = 256;

    public static int MaxSlugLength { get; set; } = 256;

    public static int MaxLayoutNameLength { get; set; } = 256;

    public static int MaxContentLength { get; set; } = int.MaxValue;

    public static int MaxScriptLength { get; set; } = int.MaxValue;

    public static int MaxStyleLength { get; set; } = int.MaxValue;
    
    public static string DefaultHomePageCacheKey { get; set; } = "__DefaultHomePage";
}
