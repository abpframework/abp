using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Fluent;

public class AbpFluentAuthorizationNodeModel : IAbpFluentAuthorizationModel
{
    public bool IsNegation { get; }

    public bool IsOrNode { get; private set; }

    /// <summary>
    /// It throws this exception if this node fails.
    /// And if this exception is null, the default exception is thrown.
    /// </summary>
    [CanBeNull]
    public Exception ExceptionForFailure { get; }

    protected List<IAbpFluentAuthorizationModel> Branches { get; } = new();

    public AbpFluentAuthorizationNodeModel(bool isNegation, [CanBeNull] Exception exceptionForFailure)
    {
        IsNegation = isNegation;
        ExceptionForFailure = exceptionForFailure;
    }

    public List<IAbpFluentAuthorizationModel> GetBranches()
    {
        return Branches;
    }

    public void AddAndBranch(IAbpFluentAuthorizationModel model)
    {
        if (Branches.Count > 0)
        {
            SetAsAndNode();
        }

        Branches.Add(model);
    }

    public void AddOrBranch(IAbpFluentAuthorizationModel model)
    {
        if (Branches.Count == 0)
        {
            throw new AbpException("Adding an OR to an empty node is not allowed.");
        }

        SetAsOrNode();
        Branches.Add(model);
    }

    private void SetAsAndNode()
    {
        if (Branches.Count > 1 && IsOrNode)
        {
            throw new AbpException(
                "Using Meet() is not allowed since the current node has been differentiated into an OR node.");
        }

        IsOrNode = false;
    }

    private void SetAsOrNode()
    {
        if (Branches.Count > 1 && !IsOrNode)
        {
            throw new AbpException(
                "Using OrMeet() is not allowed since the current node has been differentiated into an AND node.");
        }

        IsOrNode = true;
    }
}