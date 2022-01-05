using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Volo.CmsKit.Comments;

[Serializable]
public class EntityNotCommentableException : BusinessException
{
    public EntityNotCommentableException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
    {
    }

    public EntityNotCommentableException(string entityType)
    {
        Code = CmsKitErrorCodes.Comments.EntityNotCommentable;
        EntityType = entityType;
        WithData(nameof(EntityType), EntityType);
    }

    public string EntityType { get; }
}
