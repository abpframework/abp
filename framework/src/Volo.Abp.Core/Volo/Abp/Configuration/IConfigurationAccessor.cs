using System;
using Microsoft.Extensions.Configuration;

namespace Volo.Abp.Configuration
{
    [Obsolete("IConfigurationAccessor will be removed in v1.0. Use IConfiguration instead (be use that you are using generic host just like the startup templates).")]
    public interface IConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
