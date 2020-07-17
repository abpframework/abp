﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Reactions
{
    public class DefaultReactionDefinitionStore : IReactionDefinitionStore, ITransientDependency
    {
        protected CmsKitOptions Options { get; }

        public DefaultReactionDefinitionStore(IOptions<CmsKitOptions> options)
        {
            Options = options.Value;
        }

        public virtual Task<List<ReactionDefinition>> GetAvailableReactionsAsync(string entityType)
        {
            return Task.FromResult(Options.Reactions.Values.ToList());
        }

        public Task<ReactionDefinition> GetReactionOrNullAsync(string reactionName, string entityType)
        {
            return Task.FromResult(Options.Reactions.GetOrDefault(reactionName));
        }
    }
}
