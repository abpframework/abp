using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [Obsolete("Use ResolveAsync method.")]
        public virtual string Resolve(string connectionStringName = null)
        {
            return ResolveInternal(connectionStringName);
        }

        public virtual Task<string> ResolveAsync(string connectionStringName = null)
        {
            return Task.FromResult(ResolveInternal(connectionStringName));
        }

        private string ResolveInternal(string connectionStringName)
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
