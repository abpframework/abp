namespace Volo.CmsKit.Reactions
{
    public class ReactionWithSelectionDto
    {
        public ReactionDto Reaction { get; set; }

        public int Count { get; set; }

        public bool IsSelectedByCurrentUser { get; set; }
    }
}