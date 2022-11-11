using System.Collections.Generic;
using System.Reflection;
using Autofac.Core;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Autofac;

public class AbpPropertySelector : DefaultPropertySelector
{
    public AbpPropertySelector(bool preserveSetValues)
        : base(preserveSetValues)
    {
    }

    public override bool InjectProperty(PropertyInfo propertyInfo, object instance)
    {
        return propertyInfo.GetCustomAttributes(typeof(DisablePropertyInjectionAttribute), true).IsNullOrEmpty() &&
               base.InjectProperty(propertyInfo, instance);
    }

}
