using Microsoft.Extensions.Options;
using Volo.DependencyInjection;
using Volo.ExtensionMethods;
using Volo.ExtensionMethods.Collections.Generic;

namespace Volo.Abp.Data
{
    public class DefaultConnectionStringResolver : IConnectionStringResolver, ITransientDependency
    {
        private readonly DbConnectionOptions _options; //TODO: Use IOptionsSnapshot?

        public DefaultConnectionStringResolver(IOptions<DbConnectionOptions> options)
        {
            _options = options.Value;
        }

        public virtual string Resolve(string databaseName = null)
        {
            //Get module specific value if provided
            if (!databaseName.IsNullOrEmpty())
            {
                var moduleConnString = _options.ConnectionStrings.GetOrDefault(databaseName);
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