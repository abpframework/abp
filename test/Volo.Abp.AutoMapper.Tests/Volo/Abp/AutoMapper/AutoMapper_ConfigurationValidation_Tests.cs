using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.TestBase;
using Xunit;
using IObjectMapper = Volo.Abp.ObjectMapping.IObjectMapper;

namespace Volo.Abp.AutoMapper
{
    public class AutoMapper_ConfigurationValidation_Tests : AbpIntegratedTest<AutoMapper_ConfigurationValidation_Tests.Module>
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

        [DependsOn(typeof(AbpCommonModule))]
        [DependsOn(typeof(AbpAutoMapperModule))]
        public class Module : AbpModule
        {
            public override void ConfigureServices(IServiceCollection services)
            {
                services.Configure<AbpAutoMapperOptions>(options =>
                {
                    options.UseStaticMapper = false;

                    options.AddProfile<ValidatedProfile>(true);
                    options.AddProfile<NonValidatedProfile>();
                });
            }
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
    }
}
