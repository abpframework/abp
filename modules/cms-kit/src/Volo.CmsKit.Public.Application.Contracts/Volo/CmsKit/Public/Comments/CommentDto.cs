using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Volo.CmsKit.Public.Comments;

[Serializable]
public class CommentDto : ExtensibleObject, IHasConcurrencyStamp
{
    public Guid Id { get; set; }

    public string EntityType { get; set; }

    public string EntityId { get; set; }

    public string Text { get; set; }

    public Guid? RepliedCommentId { get; set; }

    public Guid CreatorId { get; set; }

    public DateTime CreationTime { get; set; }

    public CmsUserDto Author { get; set; } //TODO: Should only have AuthorId for the basic dto. see https://abp.io/docs/latest/framework/architecture/best-practices/application-services

    public string ConcurrencyStamp { get; set; }

    public string Url { get; set; }
}
