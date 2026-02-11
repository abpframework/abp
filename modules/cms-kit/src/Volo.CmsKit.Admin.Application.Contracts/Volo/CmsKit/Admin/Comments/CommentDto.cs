using System;
using Volo.Abp.ObjectExtending;

namespace Volo.CmsKit.Admin.Comments;

[Serializable]
public class CommentDto : ExtensibleObject
{
    public Guid Id { get; set; }

    public string EntityType { get; set; }

    public string EntityId { get; set; }

    public string Text { get; set; }

    public Guid? RepliedCommentId { get; set; }

    public Guid CreatorId { get; set; }

    public DateTime CreationTime { get; set; }

    public bool? IsApproved { get; set; }
}
