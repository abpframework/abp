using System;

namespace Volo.Abp;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class IntegrationServiceAttribute : Attribute
{
    public static bool IsDefinedOrInherited<T>()
    {
        return IsDefinedOrInherited(typeof(T));
    }

    public static bool IsDefinedOrInherited(Type type)
    {
        if (type.IsDefined(typeof(IntegrationServiceAttribute), true))
        {
            return true;
        }

        foreach (var @interface in type.GetInterfaces())
        {
            if (@interface.IsDefined(typeof(IntegrationServiceAttribute), true))
            {
                return true;
            }
        }
        
        return false;
    }
}