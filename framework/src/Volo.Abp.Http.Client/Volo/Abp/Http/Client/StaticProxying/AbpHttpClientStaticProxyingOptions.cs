using System;
using System.Collections.Generic;

namespace Volo.Abp.Http.Client.StaticProxying;

public class AbpHttpClientStaticProxyingOptions
{
    public List<Type> BindingFromQueryTypes { get; }

    public AbpHttpClientStaticProxyingOptions()
    {
        BindingFromQueryTypes = new List<Type>();
    }
}
