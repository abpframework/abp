using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features
{
    public abstract class FeatureCheckerBase : IFeatureChecker, ITransientDependency
    {
        public abstract Task<string> GetOrNullAsync(string name);

        public virtual async Task<bool> IsEnabledAsync(string name)
        {
            var value = await GetOrNullAsync(name);
            if (value == null)
            {
                return false;
            }

            try
            {
                return bool.Parse(value);
            }
            catch (Exception ex)
            {
                throw new AbpException(
                    $"The value '{value}' for the feature '{name}' should be a boolean, but was not!",
                    ex
                );
            }
        }
    }
}