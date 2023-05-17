using System;

namespace Volo.Abp.Image;

[AttributeUsage(AttributeTargets.Parameter)]
public class AbpOriginalImageAttribute : Attribute //TODO: Remove
{
    public AbpOriginalImageAttribute(string parameter)
    {
        Parameter = parameter;
    }

    public string Parameter { get; }
}