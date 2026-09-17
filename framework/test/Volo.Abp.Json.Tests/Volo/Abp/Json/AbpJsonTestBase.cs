using System;
using System.Collections.Concurrent;
using System.Reflection;
using Newtonsoft.Json;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Testing;

namespace Volo.Abp.Json;

public abstract class AbpJsonSystemTextJsonTestBase : AbpIntegratedTest<AbpJsonSystemTextJsonTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}

public abstract class AbpJsonNewtonsoftJsonTestBase : AbpIntegratedTest<AbpJsonNewtonsoftTestModule>
{
    protected AbpJsonNewtonsoftJsonTestBase()
    {
        var cache = typeof(AbpNewtonsoftJsonSerializer).GetField("JsonSerializerOptionsCache", BindingFlags.NonPublic | BindingFlags.Static);
        if (cache != null)
        {
            var cacheValue = cache.GetValue(null)?.As<ConcurrentDictionary<object, JsonSerializerSettings>>();
            cacheValue?.Clear();
        }
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
