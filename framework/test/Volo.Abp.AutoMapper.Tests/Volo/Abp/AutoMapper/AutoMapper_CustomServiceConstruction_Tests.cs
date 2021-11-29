using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace Volo.Abp.AutoMapper
{
    public class AutoMapper_CustomServiceConstruction_Tests : AbpIntegratedTest<AutoMapper_CustomServiceConstruction_Tests.TestModule>
    {
        private readonly IObjectMapper _objectMapper;

        public AutoMapper_CustomServiceConstruction_Tests()
        {
            _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
        }

        [Fact]
        public void Should_Custom_Service_Construction()
        {
            var source = new SourceModel
            {
                Name = nameof(SourceModel)
            };
            _objectMapper.Map<SourceModel, DestModel>(source).Name.ShouldBe(nameof(CustomMappingAction));
        }

        [DependsOn(typeof(AbpAutoMapperModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                Configure<AbpAutoMapperOptions>(options =>
                {
                    options.AddMaps<TestModule>();
                    options.Configurators.Add(configurationContext =>
                    {
                        configurationContext.MapperConfiguration.ConstructServicesUsing(type =>
                            type.Name.Contains(nameof(CustomMappingAction))
                                ? new CustomMappingAction(nameof(CustomMappingAction))
                                : Activator.CreateInstance(type));
                    });
                });
            }
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

        public class CustomMappingAction : IMappingAction<SourceModel, DestModel>
        {
            private readonly string _name;

            public CustomMappingAction(string name)
            {
                _name = name;
            }

            public void Process(SourceModel source, DestModel destination, ResolutionContext context)
            {
                destination.Name = _name;
            }
        }
    }
}
