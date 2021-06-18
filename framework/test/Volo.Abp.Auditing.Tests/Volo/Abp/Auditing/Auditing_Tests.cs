using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Volo.Abp.Auditing.App.Entities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Auditing
{
    public class Auditing_Tests : AbpAuditingTestBase
    {
        private IAuditingStore _auditingStore;
        private IAuditingManager _auditingManager;
        private IUnitOfWorkManager _unitOfWorkManager;

        public Auditing_Tests()
        {
            _auditingManager = GetRequiredService<IAuditingManager>();
            _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _auditingStore = Substitute.For<IAuditingStore>();
            services.Replace(ServiceDescriptor.Singleton(_auditingStore));
        }

        [Fact]
        public async Task Should_Write_AuditLog_For_Classes_That_Implement_IAuditingEnabled_With_Containing_Scope()
        {
            var myAuditedObject1 = GetRequiredService<MyAuditedObject1>();

            using (var scope = _auditingManager.BeginScope())
            {
                await myAuditedObject1.DoItAsync(new InputObject { Value1 = "forty-two", Value2 = 42 });
                await scope.SaveAsync();
            }

            await _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
        }

        [Fact]
        public async Task Should_Write_AuditLog_For_Classes_That_Implement_IAuditingEnabled_Without_An_Explicit_Scope()
        {
            var myAuditedObject1 = GetRequiredService<MyAuditedObject1>();

            await myAuditedObject1.DoItAsync(new InputObject { Value1 = "forty-two", Value2 = 42 });

            await _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
        }

        public interface IMyAuditedObject : ITransientDependency, IAuditingEnabled
        {

        }

        public class MyAuditedObject1 : IMyAuditedObject
        {
            public virtual Task<ResultObject> DoItAsync(InputObject inputObject)
            {
                return Task.FromResult(new ResultObject
                {
                    Value1 = inputObject.Value1 + "-result",
                    Value2 = inputObject.Value2 + 1
                });
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
                await repository.InsertAsync(new AppEntityWithAudited(Guid.NewGuid(), "test name"));
                await scope.SaveAsync();
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
                await repository.InsertAsync(new AppEntityWithAuditedAndPropertyHasDisableAuditing(Guid.NewGuid(), "test name", "test name2"));
                await scope.SaveAsync();
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
                await repository.InsertAsync(new AppEntityWithDisableAuditing(Guid.NewGuid(), "test name"));
                await scope.SaveAsync();
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Is<AuditLogInfo>(a => !a.EntityChanges.Any()));
#pragma warning restore 4014
        }

        [Fact]
        public virtual async Task Should_Write_AuditLog_For_Entity_That_Meet_Selectors()
        {
            using (var scope = _auditingManager.BeginScope())
            {
                var repository = ServiceProvider.GetRequiredService<IBasicRepository<AppEntityWithSelector, Guid>>();
                await repository.InsertAsync(new AppEntityWithSelector(Guid.NewGuid(), "test name"));
                await scope.SaveAsync();
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
                await repository.InsertAsync(new AppEntityWithPropertyHasAudited(Guid.NewGuid(), "test name"));
                await scope.SaveAsync();
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
                await repository.InsertAsync(new AppEntityWithDisableAuditingAndPropertyHasAudited(Guid.NewGuid(), "test name", "test name2", "test name3"));
                await scope.SaveAsync();
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Is<AuditLogInfo>(x =>
                x.EntityChanges.Count == 1 && x.EntityChanges[0].PropertyChanges.Count == 2 &&
                x.EntityChanges[0].PropertyChanges[0].PropertyName == nameof(AppEntityWithDisableAuditingAndPropertyHasAudited.Name) &&
                x.EntityChanges[0].PropertyChanges[1].PropertyName == nameof(AppEntityWithDisableAuditingAndPropertyHasAudited.Name3)));
#pragma warning restore 4014
        }

        [Fact]
        public virtual async Task Should_Write_AuditLog_For_Entity_That_Property_Has_Audited_Attribute_And_Has_Changed_Even_Entity_Has_DisableAuditing_Attribute()
        {
            var entityId = Guid.NewGuid();
            var repository = ServiceProvider.GetRequiredService<IBasicRepository<AppEntityWithDisableAuditingAndPropertyHasAudited, Guid>>();
            await repository.InsertAsync(new AppEntityWithDisableAuditingAndPropertyHasAudited(entityId, "test name", "test name2", "test name3"));

            using (var scope = _auditingManager.BeginScope())
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    var entity = await repository.GetAsync(entityId);
                    entity.Name = "new name1";

                    await repository.UpdateAsync(entity);

                    await uow.CompleteAsync();
                }

                await scope.SaveAsync();
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Is<AuditLogInfo>(x =>
                x.EntityChanges.Count == 1 && x.EntityChanges[0].PropertyChanges.Count == 1 &&
                x.EntityChanges[0].PropertyChanges[0].PropertyName == nameof(AppEntityWithDisableAuditingAndPropertyHasAudited.Name)));
#pragma warning restore 4014
        }

        [Fact]
        public virtual async Task Should_Write_AuditLog_If_There_No_Action_And_No_EntityChanges()
        {
            using (var scope = _auditingManager.BeginScope())
            {
                await scope.SaveAsync();
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Any<AuditLogInfo>());
#pragma warning restore 4014
        }

        private static List<string> GetBaseAuditPropertyNames()
        {
            return new List<string>
            {
                nameof(IHasCreationTime.CreationTime),
                nameof(IMustHaveCreator.CreatorId),
                nameof(IHasModificationTime.LastModificationTime),
                nameof(IModificationAuditedObject.LastModifierId),
                nameof(ISoftDelete.IsDeleted),
                nameof(IHasDeletionTime.DeletionTime),
                nameof(IDeletionAuditedObject.DeleterId)
            };
        }

        [Fact]
        public virtual async Task Should_Write_AuditLog_Ignoring_Base_Auditing_Properties_For_Entity_That_Has_Audited_Attribute()
        {
            using (var scope = _auditingManager.BeginScope())
            {
                var repository = ServiceProvider.GetRequiredService<IBasicRepository<AppFullAuditedEntityWithAudited, Guid>>();
                await repository.InsertAsync(new AppFullAuditedEntityWithAudited(Guid.NewGuid(), "test name"));
                await scope.SaveAsync();
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Is<AuditLogInfo>(x => x.EntityChanges.Count == 1
                                                                          && x.EntityChanges[0].PropertyChanges.Any(y =>
                                                                              !GetBaseAuditPropertyNames().Contains(y.PropertyName))));
#pragma warning restore 4014
        }

        [Fact]
        public virtual async Task Should_Write_AuditLog_Including_Custom_Base_Auditing_Properties_For_Entity_That_Has_Audited_Attribute()
        {
            using (var scope = _auditingManager.BeginScope())
            {
                var repository = ServiceProvider.GetRequiredService<IBasicRepository<AppEntityWithAuditedAndHasCustomAuditingProperties, Guid>>();
                await repository.InsertAsync(new AppEntityWithAuditedAndHasCustomAuditingProperties(Guid.NewGuid())
                {
                    CreationTime = DateTime.Now,
                    CreatorId = Guid.NewGuid(),
                    LastModificationTime = DateTime.Now,
                    LastModifierId = Guid.NewGuid(),
                    IsDeleted = true,
                    DeletionTime = DateTime.Now,
                    DeleterId = Guid.NewGuid()
                });
                await scope.SaveAsync();
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Is<AuditLogInfo>(x => x.EntityChanges.Count == 1
                                                                          && x.EntityChanges[0].PropertyChanges
                                                                              .Where(y => y.PropertyName != nameof(AppEntityWithAuditedAndHasCustomAuditingProperties
                                                                                  .ExtraProperties))
                                                                              .All(y => GetBaseAuditPropertyNames().Contains(y.PropertyName))));
#pragma warning restore 4014
        }

        [Fact]
        public virtual async Task Should_Write_AuditLog_Without_ExtraPropertyDictionary()
        {
            var entityId = Guid.NewGuid();
            var repository = ServiceProvider.GetRequiredService<IBasicRepository<AppEntityWithAudited, Guid>>();
            var appEntityWithAudited = new AppEntityWithAudited(entityId, "test name");
            appEntityWithAudited.SetProperty("No", 123456);
            await repository.InsertAsync(appEntityWithAudited);

            using (var scope = _auditingManager.BeginScope())
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    var entity = await repository.GetAsync(entityId);
                    entity.Name = "new test name";

                    await repository.UpdateAsync(entity);

                    await uow.CompleteAsync();
                }

                await scope.SaveAsync();
            }

#pragma warning disable 4014
            _auditingStore.Received().SaveAsync(Arg.Is<AuditLogInfo>(x => x.EntityChanges.Count == 1
                                                                          && x.EntityChanges[0].PropertyChanges.Count == 1
                                                                          && x.EntityChanges[0].PropertyChanges[0].PropertyName == nameof(AppEntityWithAudited.Name)));
#pragma warning restore 4014
        }
    }
}
