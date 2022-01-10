using System;
using System.Collections.Generic;

namespace Volo.Abp.Http.Client.ClientProxying;

public class AbpHttpClientProxyingOptions
{
    public Dictionary<Type, Type> QueryStringConverts { get; set; }

    public Dictionary<Type, Type> FormDataConverts { get; set; }

    public Dictionary<Type, Type> PathConverts { get; set; }

    public AbpHttpClientProxyingOptions()
    {
        QueryStringConverts = new Dictionary<Type, Type>();
        FormDataConverts = new Dictionary<Type, Type>();
        PathConverts = new Dictionary<Type, Type>();
    }
}
