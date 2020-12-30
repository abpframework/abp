using AutoMapper;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Testing;
using Xunit;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace Volo.Abp.AutoMapper
{
    public class AutoMapper_Dependency_Injection_Tests : AbpIntegratedTest<AutoMapperTestModule>
    {
        private readonly IObjectMapper _objectMapper;

        public AutoMapper_Dependency_Injection_Tests()
        {
            _objectMapper = GetRequiredService<IObjectMapper>();
        }

        [Fact]
        public void Should_Registered_AutoMapper_Service()
        {
            GetService<IValueResolver<string, string, string>>().ShouldBeNull();
            GetService<IMemberValueResolver<string, string, string, string>>().ShouldBeNull();
            GetService<ITypeConverter<string, string>>().ShouldBeNull();
            GetService<IValueConverter<string, string>>().ShouldBeNull();
            GetService<IMappingAction<string, string>>().ShouldBeNull();
        }

        [Fact]
        public void Custom_MappingAction_Test()
        {
            var sourceModel = new SourceModel
            {
                Name = "Source"
            };

            _objectMapper.Map<SourceModel, DestModel>(sourceModel).Name.ShouldBe(GetRequiredService<TestNameService>().Name);
        }

        public class SourceModel
        {
            public string Name { get; set; }
        }

        public class DestModel
        {
            public string Name { get; set; }
        }

        public class TestService :
            IValueResolver<string, string, string>,
            IMemberValueResolver<string, string, string, string>,
            ITypeConverter<string, string>,
            IValueConverter<string, string>,
            IMappingAction<string, string>
        {
            public string Resolve(string source, string destination, string destMember, ResolutionContext context)
            {
                return source;
            }

            public string Resolve(string source, string destination, string sourceMember, string destMember, ResolutionContext context)
            {
                return source;
            }

            public string Convert(string source, string destination, ResolutionContext context)
            {
                return source;
            }

            public string Convert(string sourceMember, ResolutionContext context)
            {
                return sourceMember;
            }

            public void Process(string source, string destination, ResolutionContext context)
            {

            }
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
            private readonly TestNameService _testNameService;

            public CustomMappingAction(TestNameService testNameService)
            {
                _testNameService = testNameService;
            }

            public void Process(SourceModel source, DestModel destination, ResolutionContext context)
            {
                destination.Name = _testNameService.Name;
            }
        }

        public class TestNameService : ITransientDependency
        {
            public string Name => nameof(TestNameService);
        }
    }
}
