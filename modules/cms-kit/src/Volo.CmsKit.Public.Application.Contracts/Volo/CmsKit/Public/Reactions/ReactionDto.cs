using JetBrains.Annotations;

namespace Volo.CmsKit.Public.Reactions
{
    public class ReactionDto
    {
        [NotNull]
        public string Name { get; set; }

        [CanBeNull]
        public string DisplayName { get; set; }
    }
}
