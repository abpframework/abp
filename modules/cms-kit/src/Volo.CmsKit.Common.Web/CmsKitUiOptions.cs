using JetBrains.Annotations;
using Volo.CmsKit.Web.Reactions;

namespace Volo.CmsKit.Web
{
    public class CmsKitUiOptions
    {
        [NotNull]
        public ReactionIconDictionary ReactionIcons { get; }

        /// <summary>
        /// Default value: "/Account/Login".
        /// </summary>
        public string LoginUrl { get; set; } //TODO: Consider to move to the ABP Framework!

        public CmsKitUiOptions()
        {
            ReactionIcons = new ReactionIconDictionary();
            LoginUrl = "/Account/Login";
        }
    }
}
