using System;
using Volo.Abp.Domain.Entities;

namespace Volo.CmsKit.Public.Comments;

[Serializable]
public class CommentDto : IHasConcurrencyStamp
{
    public Guid Id { get; set; }

    public string EntityType { get; set; }

    public string EntityId { get; set; }

    public string Text { get; set; }

    public Guid? RepliedCommentId { get; set; }

    public Guid CreatorId { get; set; }

    public DateTime CreationTime { get; set; }

    public CmsUserDto Author { get; set; } //TODO: Should only have AuthorId for the basic dto. see https://docs.abp.io/en/abp/latest/Best-Practices/Application-Services

    public string ConcurrencyStamp { get; set; }
}
