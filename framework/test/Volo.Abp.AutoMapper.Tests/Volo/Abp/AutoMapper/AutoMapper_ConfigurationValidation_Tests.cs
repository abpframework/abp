using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace Volo.Abp.AutoMapper
{
    public class MySourceClass
    {
        public string Value { get; set; }
    }

    public class MyClassValidated
    {
        public string Value { get; set; }
    }

    public class MyClassNonValidated
    {
        public string ValueNotMatched { get; set; }
    }

    public class ValidatedProfile : Profile
    {
        public ValidatedProfile()
        {
            CreateMap<MySourceClass, MyClassValidated>();
        }
    }

    public class NonValidatedProfile : Profile
    {
        public NonValidatedProfile()
        {
            CreateMap<MySourceClass, MyClassNonValidated>();
        }
    }

    public abstract class AutoMapper_ConfigurationValidation_Tests<TStartupModule> : AbpIntegratedTest<TStartupModule>
                where TStartupModule : IAbpModule
    {
        private readonly IObjectMapper _objectMapper;

        public AutoMapper_ConfigurationValidation_Tests()
        {
            _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
        }

        [Fact]
        public void Should_Validate_Configuration()
        {
            _objectMapper.Map<MySourceClass, MyClassValidated>(new MySourceClass {Value = "42"}).Value.ShouldBe("42");
            _objectMapper.Map<MySourceClass, MyClassNonValidated>(new MySourceClass {Value = "42"}).ValueNotMatched.ShouldBe(null);
        }
    }

    public class AutoMapper_ConfigurationValidation_Tests : AutoMapper_ConfigurationValidation_Tests<AutoMapper_ConfigurationValidation_Tests.TestModule>
    {
        public class ValidatedProfile : Profile
        {
            public ValidatedProfile()
            {
                CreateMap<MySourceClass, MyClassValidated>();
            }
        }

        public class NonValidatedProfile : Profile
        {
            public NonValidatedProfile()
            {
                CreateMap<MySourceClass, MyClassNonValidated>();
            }
        }

        [DependsOn(typeof(AbpAutoMapperModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                Configure<AbpAutoMapperOptions>(options =>
                {
                    options.AddProfile<ValidatedProfile>(true);
                    options.AddProfile<NonValidatedProfile>();
                });
            }
        }
    }

    public class AutoMapper_Configuration_AddProfilesFromAssembly_Tests : AutoMapper_ConfigurationValidation_Tests<AutoMapper_Configuration_AddProfilesFromAssembly_Tests.TestModule>
    {

        [DependsOn(typeof(AbpAutoMapperModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                Configure<AbpAutoMapperOptions>(options =>
                {
                    options.AddProfiles<TestModule>();
                });
            }
        }
    }
}
