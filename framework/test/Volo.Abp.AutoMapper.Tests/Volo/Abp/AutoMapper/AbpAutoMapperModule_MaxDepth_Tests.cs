using AutoMapper;
using AutoMapper.Internal;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AutoMapper.SampleClasses;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AutoMapper;

public class AbpAutoMapperModule_MaxDepth_Tests : AbpIntegratedTest<AutoMapperTestModule>
{
    private readonly IConfigurationProvider _configurationProvider;

    public AbpAutoMapperModule_MaxDepth_Tests()
    {
        _configurationProvider = ServiceProvider.GetRequiredService<IConfigurationProvider>();
    }

    [Fact]
    public void Should_Set_Default_MaxDepth_For_All_Maps()
    {
        var typeMap = _configurationProvider.Internal().FindTypeMapFor<MyEntity, MyEntityDto>();
        typeMap.ShouldNotBeNull();
        typeMap.MaxDepth.ShouldBe(64);
    }
}

public class AbpAutoMapperModule_CustomMaxDepth_Tests : AbpIntegratedTest<AbpAutoMapperModule_CustomMaxDepth_Tests.TestModule>
{
    private readonly IConfigurationProvider _configurationProvider;

    public AbpAutoMapperModule_CustomMaxDepth_Tests()
    {
        _configurationProvider = ServiceProvider.GetRequiredService<IConfigurationProvider>();
    }

    [Fact]
    public void Should_Not_Override_Custom_MaxDepth()
    {
        var typeMap = _configurationProvider.Internal().FindTypeMapFor<MyEntity, MyEntityDto>();
        typeMap.ShouldNotBeNull();
        typeMap.MaxDepth.ShouldBe(10);
    }

    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpObjectExtendingTestModule)
    )]
    public class TestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.Configurators.Add(ctx =>
                {
                    ctx.MapperConfiguration.CreateMap<MyEntity, MyEntityDto>().MaxDepth(10);
                });
            });
        }
    }
}

public class AbpAutoMapperModule_DisabledMaxDepth_Tests : AbpIntegratedTest<AbpAutoMapperModule_DisabledMaxDepth_Tests.TestModule>
{
    private readonly IConfigurationProvider _configurationProvider;

    public AbpAutoMapperModule_DisabledMaxDepth_Tests()
    {
        _configurationProvider = ServiceProvider.GetRequiredService<IConfigurationProvider>();
    }

    [Fact]
    public void Should_Not_Set_MaxDepth_When_Disabled()
    {
        var typeMap = _configurationProvider.Internal().FindTypeMapFor<MyEntity, MyEntityDto>();
        typeMap.ShouldNotBeNull();
        typeMap.MaxDepth.ShouldBe(0);
    }

    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpObjectExtendingTestModule)
    )]
    public class TestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.DefaultMaxDepth = null;
                options.Configurators.Add(ctx =>
                {
                    ctx.MapperConfiguration.CreateMap<MyEntity, MyEntityDto>();
                });
            });
        }
    }
}
