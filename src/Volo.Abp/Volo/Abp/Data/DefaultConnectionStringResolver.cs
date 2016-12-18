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

        public string Resolve(string databaseName = null)
        {
            //Get default value
            var connString = _options.ConnectionStrings.Default;

            //Override by module specific value if provided
            if (!databaseName.IsNullOrEmpty())
            {
                var moduleConnString = _options.ConnectionStrings.GetOrDefault(databaseName);
                if (!moduleConnString.IsNullOrEmpty())
                {
                    connString = moduleConnString;
                }
            }

            //TODO: Override by tenant conn string
            //TODO: Override by tenant module specific conn string

            return connString;
        }
    }
}