using System;

namespace Volo.Abp.DependencyInjection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class DisablePropertyInjectionAttribute : Attribute
{

}
