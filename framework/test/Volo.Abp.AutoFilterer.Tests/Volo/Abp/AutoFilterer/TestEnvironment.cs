using AutoFilterer.Attributes;
using AutoFilterer.Enums;
using AutoFilterer.Types;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.MemoryDb;
using Volo.Abp.Linq;
using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace Volo.Abp.AutoFilterer
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAutoFiltererModule)
        )]
    public class TestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureSwaggerServices(context.Services);
            ConfigureAutoMapper();
            context.Services.AddTransient<IAsyncQueryableExecuter, AsyncQueryableExecuter>();
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Bookstore API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );
        }

        private void ConfigureAutoMapper()
        {
            Configure<AbpAutoMapperOptions>(options => options.AddMaps<TestModule>());
        }
    }

    public class TestDbContext : MemoryDbContext
    {
        public DbSet<Book> Books { get; set; }
    }
    public class Book : AuditedAggregateRoot<Guid>
    {
        public string Title { get; set; }
        public int TotalPage { get; set; }
        public DateTime PublishDate { get; set; }
    }

    [AutoMap(typeof(Book), ReverseMap = true)]
    public class BookDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public int TotalPage { get; set; }
        public DateTime PublishDate { get; set; }
    }

    public class BookFilterAbpFilterBaseDto : AbpFilterBase
    {
        [CompareTo("Title")]
        [ToLowerContainsComparison]
        public string Filter { get; set; }
    }

    public class MockingMemoryRepository<TDbContext, TEntity, TKey> : MemoryDbRepository<TDbContext, TEntity, TKey>
        where TDbContext : MemoryDbContext
        where TEntity : class, IEntity<TKey>
    {
        private readonly IQueryable<TEntity> seed;

        public MockingMemoryRepository(IMemoryDatabaseProvider<TDbContext> databaseProvider, IQueryable<TEntity> seed) : base(databaseProvider)
        {
            this.seed = seed;
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return seed;
        }
    }
}
