using System;
using System.Collections.Generic;

namespace Volo.Abp.Identity;

public class IdentitySecurityLogContext
{
    public string Identity { get; set; }

    public string Action { get; set; }

    public string UserName { get; set; }

    public string ClientId { get; set; }

    public Dictionary<string, object> ExtraProperties { get; }

    public IdentitySecurityLogContext()
    {
        ExtraProperties = new Dictionary<string, object>();
    }

    public virtual IdentitySecurityLogContext WithProperty(string key, object value)
    {
        ExtraProperties[key] = value;
        return this;
    }

}
