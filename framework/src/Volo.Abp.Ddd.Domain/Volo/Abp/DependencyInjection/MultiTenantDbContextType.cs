using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.DependencyInjection;

public class MultiTenantDbContextType
{
    public Type Type { get; }

    public MultiTenancySides MultiTenancySide { get; }

    public MultiTenantDbContextType(Type type, MultiTenancySides multiTenancySide = MultiTenancySides.Both)
    {
        Type = type;
        MultiTenancySide = multiTenancySide;
    }

    public override bool Equals(object obj)
    {
        var other = obj as MultiTenantDbContextType;

        if (other == null)
        {
            return false;
        }

        return other.Type == Type && other.MultiTenancySide == MultiTenancySide;
    }

    public override int GetHashCode()
    {
        return Type.GetHashCode() ^ MultiTenancySide.GetHashCode();
    }
}
