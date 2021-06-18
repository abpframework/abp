using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Options
{
    public abstract class AbpDynamicOptionsManager<T> : OptionsManager<T>
        where T : class
    {
        protected AbpDynamicOptionsManager(IOptionsFactory<T> factory)
            : base(factory)
        {

        }

        public Task SetAsync() => SetAsync(Microsoft.Extensions.Options.Options.DefaultName);

        public virtual Task SetAsync(string name)
        {
            return OverrideOptionsAsync(name, base.Get(name));
        }

        protected abstract Task OverrideOptionsAsync(string name, T options);
    }
}
