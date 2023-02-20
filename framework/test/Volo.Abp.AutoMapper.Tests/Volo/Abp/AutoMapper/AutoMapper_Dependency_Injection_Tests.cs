using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Testing;
using Xunit;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace Volo.Abp.AutoMapper;

public class AutoMapper_Dependency_Injection_Tests : AbpIntegratedTest<AutoMapperTestModule>
{
    [Fact]
    public void Should_Registered_AutoMapper_Service()
    {
        GetService<CustomMappingAction>().ShouldNotBeNull();
    }

    [Fact]
    public void Custom_MappingAction_Test()
    {
        var sourceModel = new SourceModel
        {
            Name = "Source"
        };

        using (var scope = ServiceProvider.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<IObjectMapper>().Map<SourceModel, DestModel>(sourceModel).Name.ShouldBe(nameof(CustomMappingActionService));
        }

        CustomMappingAction.IsDisposed.ShouldBeTrue();
    }

    public class SourceModel
    {
        public string Name { get; set; }
    }

    public class DestModel
    {
        public string Name { get; set; }
    }

    public class MapperActionProfile : Profile
    {
        public MapperActionProfile()
        {
            CreateMap<SourceModel, DestModel>().AfterMap<CustomMappingAction>();
        }
    }

    public class CustomMappingAction : IMappingAction<SourceModel, DestModel>, IDisposable
    {
        public static bool IsDisposed = false;

        private readonly CustomMappingActionService _customMappingActionService;

        public CustomMappingAction(CustomMappingActionService customMappingActionService)
        {
            _customMappingActionService = customMappingActionService;
        }

        public void Process(SourceModel source, DestModel destination, ResolutionContext context)
        {
            destination.Name = _customMappingActionService.GetName();
        }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    public class CustomMappingActionService : ITransientDependency
    {
        public string GetName()
        {
            return nameof(CustomMappingActionService);
        }
    }
}
