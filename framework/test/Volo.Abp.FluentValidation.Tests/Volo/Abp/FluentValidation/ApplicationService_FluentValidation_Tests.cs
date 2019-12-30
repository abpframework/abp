using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Autofac;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Volo.Abp.Validation;
using Xunit;

namespace Volo.Abp.FluentValidation
{
    public class ApplicationService_FluentValidation_Tests : AbpIntegratedTest<ApplicationService_FluentValidation_Tests.TestModule>
    {
        private readonly IMyAppService _myAppService;

        public ApplicationService_FluentValidation_Tests()
        {
            _myAppService = ServiceProvider.GetRequiredService<IMyAppService>();
        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        [Fact]
        public async Task Should_Work_Proper_With_Right_Inputs()
        {
            var asyncOutput = await _myAppService.MyMethodAsync(new MyMethodInput
            {
                MyStringValue = "aaa",
                MyMethodInput2 = new MyMethodInput2
                {
                    MyStringValue2 = "bbb"
                },
                MyMethodInput3 = new MyMethodInput3
                {
                    MyStringValue3 = "ccc"
                }
            });

            asyncOutput.ShouldBe("aaabbbccc");
        }

        [Fact]
        public async Task Should_Not_Work_With_Wrong_Inputs()
        {
            // MyStringValue should be aaa, MyStringValue2 should be bbb. MyStringValue3 should be ccc

            var exception = await Assert.ThrowsAsync<AbpValidationException>(
                async () => await _myAppService.MyMethodAsync(
                    new MyMethodInput
                    {
                        MyStringValue = "a",
                        MyMethodInput2 = new MyMethodInput2
                        {
                            MyStringValue2 = "b"
                        },
                        MyMethodInput3 = new MyMethodInput3
                        {
                            MyStringValue3 = "c"
                        }
                    }
                )
            );
            
            exception.ValidationErrors.ShouldContain(x => x.MemberNames.Contains("MyStringValue"));
            exception.ValidationErrors.ShouldContain(x => x.MemberNames.Contains("MyMethodInput2.MyStringValue2"));
            exception.ValidationErrors.ShouldContain(x => x.MemberNames.Contains("MyMethodInput3.MyStringValue3"));

            exception = await Assert.ThrowsAsync<AbpValidationException>(async () => await _myAppService.MyMethodAsync(
                new MyMethodInput
                {
                    MyStringValue = "a",
                    MyMethodInput2 = new MyMethodInput2
                    {
                        MyStringValue2 = "b"
                    },
                    MyMethodInput3 = new MyMethodInput3
                    {
                        MyStringValue3 = "c"
                    }
                }));
            exception.ValidationErrors.ShouldContain(x => x.MemberNames.Contains("MyStringValue"));
            exception.ValidationErrors.ShouldContain(x => x.MemberNames.Contains("MyMethodInput2.MyStringValue2"));
            exception.ValidationErrors.ShouldContain(x => x.MemberNames.Contains("MyMethodInput3.MyStringValue3"));
        }

        [Fact]
        public async Task NotValidateMyMethod_Test()
        {
            var output = await _myAppService.NotValidateMyMethod(new MyMethodInput4
            {
                MyStringValue4 = "444"
            });

            output.ShouldBe("444");
        }
        
        [DependsOn(typeof(AbpAutofacModule))]
        [DependsOn(typeof(AbpFluentValidationModule))]
        public class TestModule : AbpModule
        {
            public override void PreConfigureServices(ServiceConfigurationContext context)
            {
                context.Services.OnRegistred(onServiceRegistredContext =>
                {
                    if (typeof(IMyAppService).IsAssignableFrom(onServiceRegistredContext.ImplementationType))
                    {
                        onServiceRegistredContext.Interceptors.TryAdd<ValidationInterceptor>();
                    }
                });
            }

            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                context.Services.AddType<MyAppService>();
            }
        }

        public interface IMyAppService
        {
            Task<string> MyMethodAsync(MyMethodInput input);

            Task<string> NotValidateMyMethod(MyMethodInput4 input);
        }

        public class MyAppService : IMyAppService, ITransientDependency
        {
            public Task<string> MyMethodAsync(MyMethodInput input)
            {
                return Task.FromResult(input.MyStringValue + input.MyMethodInput2.MyStringValue2 +
                                       input.MyMethodInput3.MyStringValue3);
            }

            public Task<string> NotValidateMyMethod(MyMethodInput4 input)
            {
                return Task.FromResult(input.MyStringValue4);
            }
        }

        public class MyMethodInput
        {
            public string MyStringValue { get; set; }

            public MyMethodInput2 MyMethodInput2 { get; set; }

            public MyMethodInput3 MyMethodInput3 { get; set; }
        }

        public class MyMethodInput2
        {
            public string MyStringValue2 { get; set; }
        }

        public class MyMethodInput3
        {

            public string MyStringValue3 { get; set; }
        }

        public class MyMethodInput4
        {
            public string MyStringValue4 { get; set; }
        }

        public class MyMethodInputValidator : AbstractValidator<MyMethodInput>
        {
            public MyMethodInputValidator()
            {
                RuleFor(x => x.MyStringValue).Equal("aaa");
                RuleFor(x => x.MyMethodInput2.MyStringValue2).Equal("bbb");
                RuleFor(customer => customer.MyMethodInput3).SetValidator(new MyMethodInput3Validator());
            }
        }

        public class MethodInputBaseValidator : AbstractValidator<MyMethodInput3>
        {
            public MethodInputBaseValidator()
            {
                RuleFor(x => x.MyStringValue3).NotNull();
            }
        }

        public class MyMethodInput3Validator : MethodInputBaseValidator
        {
            public MyMethodInput3Validator()
            {
                RuleFor(x => x.MyStringValue3).Equal("ccc");
            }
        }
    }
}