using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;

namespace Volo.CmsKit;

public abstract class PolicySpecifiedDefinition : EntityTypeDefinition, IEquatable<PolicySpecifiedDefinition>
{
    public PolicySpecifiedDefinition(
        [NotNull] string entityType,
        IEnumerable<string> createPolicies = null,
        IEnumerable<string> updatePolicies = null,
        IEnumerable<string> deletePolicies = null) : base(entityType)
    {
        if (createPolicies != null)
        {
            CreatePolicies = CreatePolicies.Concat(createPolicies).ToList();
        }

        if (updatePolicies != null)
        {
            UpdatePolicies = UpdatePolicies.Concat(updatePolicies).ToList();
        }

        if (deletePolicies != null)
        {
            DeletePolicies = DeletePolicies.Concat(deletePolicies).ToList();
        }
    }

    [NotNull]
    public virtual ICollection<string> CreatePolicies { get; } = new List<string>();

    [NotNull]
    public virtual ICollection<string> UpdatePolicies { get; } = new List<string>();

    [NotNull]
    public virtual ICollection<string> DeletePolicies { get; } = new List<string>();

    public bool Equals(PolicySpecifiedDefinition other)
    {
        return other?.EntityType == EntityType;
    }
}
