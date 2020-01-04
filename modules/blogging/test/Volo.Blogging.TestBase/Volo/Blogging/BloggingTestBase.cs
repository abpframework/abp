using System;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Users;
using Volo.Abp.Testing;

namespace Volo.Blogging
{
    public abstract class BloggingTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected Guid? CurrentUserId { get; set; }

        protected BloggingTestBase()
        {
            CurrentUserId = Guid.NewGuid();
        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            var currentUser = Substitute.For<ICurrentUser>();
            currentUser.Id.Returns(ci => CurrentUserId);
            services.AddSingleton(currentUser);
        }
    }
}
