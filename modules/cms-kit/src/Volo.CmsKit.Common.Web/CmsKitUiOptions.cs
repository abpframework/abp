using JetBrains.Annotations;
using Volo.CmsKit.Web.MarkedItems;
using Volo.CmsKit.Web.Reactions;

namespace Volo.CmsKit.Web;

public class CmsKitUiOptions
{
    [NotNull]
    public ReactionIconDictionary ReactionIcons { get; }

    public CmsKitUiCommentOptions CommentsOptions { get; }

    public MarkedItemIconDictionary MarkedItemIcons { get; }

    public CmsKitUiOptions()
    {
        ReactionIcons = new ReactionIconDictionary();
        CommentsOptions = new CmsKitUiCommentOptions();
        MarkedItemIcons = new MarkedItemIconDictionary();
    }
}
