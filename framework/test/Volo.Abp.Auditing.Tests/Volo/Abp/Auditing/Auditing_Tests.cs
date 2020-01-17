using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Volo.Abp.Auditing.App.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace Volo.Abp.Auditing
{
    public class Auditing_Tests : AbpAuditingTestBase
    {
        private IAuditingStore _auditingStore;
        private IAuditingManager _auditingManager;

        public Auditing_Tests()
        {
            _auditingManager = GetRequiredService<IAuditingManager>();
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
                await myAuditedObject1.DoItAsync(new InputObject { Value1 = "forty-two", Value2 = 42 }).ConfigureAwait(false);
                await scope.SaveAsync().ConfigureAwait(false);
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
#pragma warning restore 4014
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

        [Fact]
        public virtual async Task Should_Write_AuditLog_For_Entity_That_Has_Audited_Attribute()
        {
            using (var scope = _auditingManager.BeginScope())
            {
                var repository = ServiceProvider.GetRequiredService<IBasicRepository<AppEntityWithAudited, Guid>>();
                await repository.InsertAsync(new AppEntityWithAudited(Guid.NewGuid(), "test name")).ConfigureAwait(false);
                await scope.SaveAsync().ConfigureAwait(false);
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
#pragma warning restore 4014
        }

        [Fact]
        public virtual async Task Should_Not_Write_AuditLog_For_Property_That_Has_DisableAuditing_Attribute()
        {
            using (var scope = _auditingManager.BeginScope())
            {
                var repository = ServiceProvider.GetRequiredService<IBasicRepository<AppEntityWithAuditedAndPropertyHasDisableAuditing, Guid>>();
                await repository.InsertAsync(new AppEntityWithAuditedAndPropertyHasDisableAuditing(Guid.NewGuid(), "test name", "test name2")).ConfigureAwait(false);
                await scope.SaveAsync().ConfigureAwait(false);
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Is<AuditLogInfo>(x =>
                x.EntityChanges.Count == 1 &&
                !(x.EntityChanges[0].PropertyChanges.Any(p =>
                    p.PropertyName == nameof(AppEntityWithDisableAuditingAndPropertyHasAudited.Name2)))));
#pragma warning restore 4014
        }

        [Fact]
        public virtual async Task Should_Not_Write_AuditLog_For_Entity_That_Has_DisableAuditing_Attribute()
        {
            using (var scope = _auditingManager.BeginScope())
            {
                var repository = ServiceProvider.GetRequiredService<IBasicRepository<AppEntityWithDisableAuditing, Guid>>();
                await repository.InsertAsync(new AppEntityWithDisableAuditing(Guid.NewGuid(), "test name")).ConfigureAwait(false);
                await scope.SaveAsync().ConfigureAwait(false);
            }

#pragma warning disable 4014
            _auditingStore.DidNotReceive().SaveAsync(Arg.Any<AuditLogInfo>());
#pragma warning restore 4014
        }

        [Fact]
        public virtual async Task Should_Write_AuditLog_For_Entity_That_Meet_Selectors()
        {
            using (var scope = _auditingManager.BeginScope())
            {
                var repository = ServiceProvider.GetRequiredService<IBasicRepository<AppEntityWithSelector, Guid>>();
                await repository.InsertAsync(new AppEntityWithSelector(Guid.NewGuid(), "test name")).ConfigureAwait(false);
                await scope.SaveAsync().ConfigureAwait(false);
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
#pragma warning restore 4014
        }

        [Fact]
        public virtual async Task Should_Write_AuditLog_For_Entity_That_Property_Has_Audited_Attribute()
        {
            using (var scope = _auditingManager.BeginScope())
            {
                var repository = ServiceProvider.GetRequiredService<IBasicRepository<AppEntityWithPropertyHasAudited, Guid>>();
                await repository.InsertAsync(new AppEntityWithPropertyHasAudited(Guid.NewGuid(), "test name")).ConfigureAwait(false);
                await scope.SaveAsync().ConfigureAwait(false);
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
#pragma warning restore 4014
        }

        [Fact]
        public virtual async Task Should_Write_AuditLog_For_Entity_That_Property_Has_Audited_Attribute_Even_Entity_Has_DisableAuditing_Attribute()
        {
            using (var scope = _auditingManager.BeginScope())
            {
                var repository = ServiceProvider.GetRequiredService<IBasicRepository<AppEntityWithDisableAuditingAndPropertyHasAudited, Guid>>();
                await repository.InsertAsync(new AppEntityWithDisableAuditingAndPropertyHasAudited(Guid.NewGuid(), "test name", "test name2")).ConfigureAwait(false);
                await scope.SaveAsync().ConfigureAwait(false);
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Is<AuditLogInfo>(x =>
                x.EntityChanges.Count == 1 && x.EntityChanges[0].PropertyChanges.Count == 1 &&
                x.EntityChanges[0].PropertyChanges[0].PropertyName ==
                nameof(AppEntityWithDisableAuditingAndPropertyHasAudited.Name)));
#pragma warning restore 4014
        }
    }
}
