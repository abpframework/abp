using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Data
{
    public class DefaultConnectionStringResolver : IConnectionStringResolver, ITransientDependency
    {
        protected AbpDbConnectionOptions Options { get; }

        public DefaultConnectionStringResolver(IOptionsSnapshot<AbpDbConnectionOptions> options)
        {
            Options = options.Value;
        }

        public virtual string Resolve(string connectionStringName = null)
        {
            //Get module specific value if provided
            if (!connectionStringName.IsNullOrEmpty())
            {
                var moduleConnString = Options.ConnectionStrings.GetOrDefault(connectionStringName);
                if (!moduleConnString.IsNullOrEmpty())
                {
                    return moduleConnString;
                }
            }
            
            //Get default value
            return Options.ConnectionStrings.Default;
        }
    }
}