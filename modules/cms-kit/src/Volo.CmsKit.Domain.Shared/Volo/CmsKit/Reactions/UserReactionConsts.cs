using Volo.CmsKit.Entities;

namespace Volo.CmsKit.Reactions;

public static class UserReactionConsts
{
    public static int MaxEntityTypeLength { get; set; } = CmsEntityConsts.MaxEntityTypeLength;

    public static int MaxEntityIdLength { get; set; } = CmsEntityConsts.MaxEntityIdLength;

    public static int MaxReactionNameLength { get; set; } = 32;
}
