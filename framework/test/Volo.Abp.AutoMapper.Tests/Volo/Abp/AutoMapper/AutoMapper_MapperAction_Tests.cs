using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Testing;
using Xunit;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace Volo.Abp.AutoMapper
{
    public class AutoMapper_MapperAction_Tests : AbpIntegratedTest<AutoMapperTestModule>
    {
        private readonly IObjectMapper _objectMapper;

        public AutoMapper_MapperAction_Tests()
        {
            _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
        }

        [Fact]
        public void Should_Name_DIMappingAction()
        {
            var mapperActionEntity = new MapperActionModel {Name = "MapperActionEntity"};
            _objectMapper.Map<MapperActionModel, MapperActionModel2>(mapperActionEntity)
                .Name.ShouldBe(nameof(DIMappingAction));
        }

        [Fact]
        public void Should_Name_NotDIMappingAction()
        {
            var mapperActionEntity = new MapperActionModel2 {Name = "MapperActionEntity"};
            _objectMapper.Map<MapperActionModel2, MapperActionModel>(mapperActionEntity)
                .Name.ShouldBe(nameof(NotDIMapperAction));
        }

        public class MapperActionModel
        {
            public string Name { get; set; }
        }

        public class MapperActionModel2
        {
            public string Name { get; set; }
        }

        public class MapperActionProfile : Profile
        {
            public MapperActionProfile()
            {
                CreateMap<MapperActionModel, MapperActionModel2>().AfterMap<DIMappingAction>();
                CreateMap<MapperActionModel2, MapperActionModel>().AfterMap<NotDIMapperAction>();
            }
        }

        public class DIMappingAction : IMappingAction<MapperActionModel, MapperActionModel2>
        {
            private readonly MapperActionService _mapperActionService;

            public DIMappingAction(MapperActionService mapperActionService)
            {
                _mapperActionService = mapperActionService;
            }

            public void Process(MapperActionModel source, MapperActionModel2 destination, ResolutionContext context)
            {
                destination.Name = _mapperActionService.GetName();
            }
        }

        public class NotDIMapperAction : IMappingAction<MapperActionModel2, MapperActionModel>
        {
            public void Process(MapperActionModel2 source, MapperActionModel destination,
                ResolutionContext context)
            {
                destination.Name = nameof(NotDIMapperAction);
            }
        }

        public class MapperActionService : ITransientDependency
        {
            public string GetName()
            {
                return nameof(DIMappingAction);
            }
        }
    }
}
