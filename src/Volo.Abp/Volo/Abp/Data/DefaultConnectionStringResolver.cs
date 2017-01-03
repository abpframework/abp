using Microsoft.Extensions.Options;
using Volo.DependencyInjection;
using Volo.ExtensionMethods;
using Volo.ExtensionMethods.Collections.Generic;

namespace Volo.Abp.Data
{
    public class DefaultConnectionStringResolver : IConnectionStringResolver, ITransientDependency
    {
        private readonly DbConnectionOptions _options;

        public DefaultConnectionStringResolver(IOptionsSnapshot<DbConnectionOptions> options)
        {
            _options = options.Value;
        }

        public virtual string Resolve(string connectionStringName = null)
        {
            //Get module specific value if provided
            if (!connectionStringName.IsNullOrEmpty())
            {
                var moduleConnString = _options.ConnectionStrings.GetOrDefault(connectionStringName);
                if (!moduleConnString.IsNullOrEmpty())
                {
                    return moduleConnString;
                }
            }
            
            //Get default value
            return _options.ConnectionStrings.Default;
        }
    }
}