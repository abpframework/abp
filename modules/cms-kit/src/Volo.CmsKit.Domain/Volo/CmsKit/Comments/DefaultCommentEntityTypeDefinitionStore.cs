using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Comments;

public class DefaultCommentEntityTypeDefinitionStore : ICommentEntityTypeDefinitionStore
{
    protected CmsKitCommentOptions Options { get; }

    public DefaultCommentEntityTypeDefinitionStore(IOptions<CmsKitCommentOptions> options)
    {
        Options = options.Value;
    }

    public virtual Task<CommentEntityTypeDefinition> GetAsync([NotNull] string entityType)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

        var result = Options.EntityTypes.SingleOrDefault(x => x.EntityType.Equals(entityType, StringComparison.InvariantCultureIgnoreCase)) ??
                     throw new EntityNotCommentableException(entityType);

        return Task.FromResult(result);
    }

    public virtual Task<bool> IsDefinedAsync([NotNull] string entityType)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

        var isDefined = Options.EntityTypes.Any(x => x.EntityType == entityType);

        return Task.FromResult(isDefined);
    }
}
