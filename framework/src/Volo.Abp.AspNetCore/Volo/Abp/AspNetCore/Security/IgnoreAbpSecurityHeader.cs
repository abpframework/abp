using System;

namespace Volo.Abp.AspNetCore.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class IgnoreAbpSecurityHeaderAttribute : Attribute
{
    
}
