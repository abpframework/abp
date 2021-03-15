using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace Volo.CmsKit.Reactions
{
    public class ReactionEntityTypeDefinition : PolicySpecifiedDefinition
    {
        [NotNull]
        public List<ReactionDefinition> Reactions { get; } = new();

        public ReactionEntityTypeDefinition(
            [NotNull] string entityType,
            [NotNull] IEnumerable<ReactionDefinition> reactions,
            IEnumerable<string> createPolicies = null,
            IEnumerable<string> updatePolicies = null,
            IEnumerable<string> deletePolicies = null) : base(entityType, createPolicies, updatePolicies, deletePolicies)
        {
            Reactions = Check.NotNull(reactions, nameof(reactions)).ToList();
        }
    }
}
