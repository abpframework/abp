using Volo.CmsKit.Entities;

namespace Volo.CmsKit.Comments;

public static class CommentConsts
{
    public const string EntityType = "Comment";
    public static int MaxEntityTypeLength { get; set; } = CmsEntityConsts.MaxEntityTypeLength;

    public static int MaxEntityIdLength { get; set; } = CmsEntityConsts.MaxEntityIdLength;

    public static int MaxTextLength { get; set; } = 512;
    
    public static int MaxUrlLength { get; set; } = 512;

    public static int MaxIdempotencyTokenLength { get; set; } = 32;
}
