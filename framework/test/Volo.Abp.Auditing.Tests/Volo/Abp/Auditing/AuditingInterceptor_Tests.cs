using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Volo.Abp.Autofac;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Auditing
{
    public class AuditingInterceptor_Tests : AbpIntegratedTest<AuditingInterceptor_Tests.TestModule>
    {
        private IAuditingStore _auditingStore;
        private IAuditingManager _auditingManager;

        public AuditingInterceptor_Tests()
        {
            _auditingManager = GetRequiredService<IAuditingManager>();
        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _auditingStore = Substitute.For<IAuditingStore>();
            services.Replace(ServiceDescriptor.Singleton(_auditingStore));
        }

        [Fact]
        public async Task Should_Write_AuditLog_For_Classes_That_Implement_IAuditingEnabled()
        {
            var myAuditedObject1 = GetRequiredService<MyAuditedObject1>();

            using (var scope = _auditingManager.BeginScope())
            {
                await myAuditedObject1.DoItAsync(new InputObject { Value1 = "forty-two", Value2 = 42 });
                await scope.SaveAsync();
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
#pragma warning restore 4014
        }

        [DependsOn(
            typeof(AbpAuditingModule),
            typeof(AbpAutofacModule)
            )]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                context.Services.AddType<MyAuditedObject1>();
            }
        }

        public interface IMyAuditedObject : ITransientDependency, IAuditingEnabled
        {

        }

        public class MyAuditedObject1 : IMyAuditedObject
        {
            public async virtual Task<ResultObject> DoItAsync(InputObject inputObject)
            {
                return new ResultObject
                {
                    Value1 = inputObject.Value1 + "-result",
                    Value2 = inputObject.Value2 + 1
                };
            }
        }

        public class ResultObject
        {
            public string Value1 { get; set; }

            public int Value2 { get; set; }
        }

        public class InputObject
        {
            public string Value1 { get; set; }

            public int Value2 { get; set; }
        }
    }
}
