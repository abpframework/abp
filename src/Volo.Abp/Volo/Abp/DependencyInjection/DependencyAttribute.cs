using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Application.Services;
using Volo.Abp.Modularity;

namespace Volo.Abp.DependencyInjection
{
    public class DependencyAttribute : Attribute
    {
        public virtual ServiceLifetime? Lifetime { get; set; }

        public virtual bool TryRegister { get; set; }

        public virtual bool ReplaceServices { get; set; }

        public DependencyAttribute()
        {

        }

        public DependencyAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }

    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ITaxCalculator))]
    public class TaxCalculator : ITaxCalculator, ITransientDependency
    {

    }

    public interface ITaxCalculator
    {
    }

    public class BlogModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            //Add single type
            services.AddType(typeof(TaxCalculator));

            //Add multiple types in once call
            services.AddTypes(typeof(TaxCalculator), typeof(MyOtherService));

            //Add single type generic shortcut
            services.AddType<TaxCalculator>();
        }
    }

    public class MyService : ITransientDependency
    {
        public ILogger<MyService> Logger { get; set; }

        public MyService()
        {
            Logger = NullLogger<MyService>.Instance;
        }

        public void DoSomething()
        {
            //...use Logger to write logs...
        }
    }
}