using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.Domain.Entities.Events.Distributed.ExternalEntitySynchronizers;

public class ExternalEntitySynchronizer_Tests : AbpIntegratedTest<ExternalEntitySynchronizer_Tests.TestModule>
{
    [Fact]
    public async Task Should_Handle_Entity_Created_Event()
    {
        var bookId = Guid.NewGuid();

        var uowManager = GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowManager.Begin();

        var bookSynchronizer = GetRequiredService<BookSynchronizer>();
        var repository = GetRequiredService<IRepository<Book, Guid>>();

        (await repository.FindAsync(bookId)).ShouldBeNull();

        var remoteBookEto = new RemoteBookEto { KeysAsString = bookId.ToString(), EntityVersion = 0, Sold = 1 };

        await bookSynchronizer.HandleEventAsync(new EntityCreatedEto<RemoteBookEto>(remoteBookEto));

        var book = await repository.FindAsync(bookId);
        book.ShouldNotBeNull();
        book.EntityVersion.ShouldBe(remoteBookEto.EntityVersion);
        book.Sold.ShouldBe(1);
    }

    [Fact]
    public async Task Should_Handle_Entity_Update_Event()
    {
        var bookId = Guid.NewGuid();

        var uowManager = GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowManager.Begin();

        var bookSynchronizer = GetRequiredService<BookSynchronizer>();
        var repository = GetRequiredService<IRepository<Book, Guid>>();

        (await repository.FindAsync(bookId)).ShouldBeNull();

        var remoteBookEto = new RemoteBookEto { KeysAsString = bookId.ToString(), EntityVersion = 0, Sold = 1 };

        await bookSynchronizer.HandleEventAsync(new EntityUpdatedEto<RemoteBookEto>(remoteBookEto));

        var book = await repository.FindAsync(bookId);
        book.ShouldNotBeNull();
        book.EntityVersion.ShouldBe(remoteBookEto.EntityVersion);
        book.Sold.ShouldBe(1);

        remoteBookEto.EntityVersion = 1;
        remoteBookEto.Sold = 2;

        await bookSynchronizer.HandleEventAsync(new EntityUpdatedEto<RemoteBookEto>(remoteBookEto));

        book = await repository.FindAsync(bookId);
        book.ShouldNotBeNull();
        book.EntityVersion.ShouldBe(remoteBookEto.EntityVersion);
        book.Sold.ShouldBe(2);

        remoteBookEto.EntityVersion = 0;
        remoteBookEto.Sold = 3;

        await bookSynchronizer.HandleEventAsync(new EntityUpdatedEto<RemoteBookEto>(remoteBookEto));

        // Should skip synchronizing older remote entities.
        book = await repository.FindAsync(bookId);
        book.ShouldNotBeNull();
        book.EntityVersion.ShouldBe(1);
        book.Sold.ShouldBe(2);
    }

    [Fact]
    public async Task Should_Handle_Entity_Deleted_Event()
    {
        var bookId = Guid.NewGuid();

        var uowManager = GetRequiredService<IUnitOfWorkManager>();
        using var uow = uowManager.Begin();

        var bookSynchronizer = GetRequiredService<BookSynchronizer>();
        var repository = GetRequiredService<IRepository<Book, Guid>>();

        await repository.InsertAsync(new Book(bookId, 1, 0), true);

        var book = await repository.FindAsync(bookId);
        book.ShouldNotBeNull();
        book.Id.ShouldBe(bookId);
        book.EntityVersion.ShouldBe(0);

        var remoteBookEto = new RemoteBookEto { KeysAsString = bookId.ToString(), EntityVersion = 0, Sold = 1 };

        await bookSynchronizer.HandleEventAsync(new EntityDeletedEto<RemoteBookEto>(remoteBookEto));

        (await repository.FindAsync(bookId)).ShouldBeNull();

        await bookSynchronizer.HandleEventAsync(new EntityDeletedEto<RemoteBookEto>(remoteBookEto));

        (await repository.FindAsync(bookId)).ShouldBeNull();
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpMemoryDbModule),
        typeof(AbpDddDomainModule),
        typeof(AbpAutoMapperModule)
    )]
    public class TestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var connStr = Guid.NewGuid().ToString();

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connStr;
            });

            context.Services.AddMemoryDbContext<MyMemoryDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            context.Services.AddAutoMapperObjectMapper<TestModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<TestModule>(validate: true);
            });
        }
    }

    public class MyMemoryDbContext : MemoryDbContext
    {
        public override IReadOnlyList<Type> GetEntityTypes()
        {
            return new List<Type> { typeof(Book) };
        }
    }

    public class MyAutoMapperProfile : Profile
    {
        public MyAutoMapperProfile()
        {
            CreateMap<RemoteBookEto, Book>(MemberList.None)
                .ForMember(x => x.Id, options => options.MapFrom(x => Guid.Parse(x.KeysAsString)));
        }
    }
}