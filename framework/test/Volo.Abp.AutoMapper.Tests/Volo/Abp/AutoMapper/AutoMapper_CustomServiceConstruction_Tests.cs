using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AutoMapper.SampleClasses;
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
            var myEntity = new MyEntity
            {
                Number = 2
            };
            _objectMapper.Map<MyEntity, MyEntityDto>(myEntity).Number.ShouldBe(1);
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
                                ? new CustomMappingAction(1)
                                : Activator.CreateInstance(type));
                    });
                });
            }
        }

        public class MapperActionProfile : Profile
        {
            public MapperActionProfile()
            {
                CreateMap<MyEntity, MyEntityDto>().AfterMap<CustomMappingAction>();
            }
        }

        public class CustomMappingAction : IMappingAction<MyEntity, MyEntityDto>
        {
            private readonly int _number;

            public CustomMappingAction(int number)
            {
                _number = number;
            }

            public void Process(MyEntity source, MyEntityDto destination, ResolutionContext context)
            {
                destination.Number = _number;
            }
        }
    }
}
