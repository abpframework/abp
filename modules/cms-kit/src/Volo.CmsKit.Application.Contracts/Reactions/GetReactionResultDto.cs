using System.Collections.Generic;

namespace Volo.CmsKit.Reactions
{
    public class GetReactionResultDto
    {
        public List<ReactionCountDto> AllReactions { get; set; }

        //TODO: Reactions of the current user (if available)
    }
}
