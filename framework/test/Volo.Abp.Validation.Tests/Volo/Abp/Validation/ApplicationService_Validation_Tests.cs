using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Autofac;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Validation
{
    public class ApplicationService_Validation_Tests : AbpIntegratedTest<ApplicationService_Validation_Tests.TestModule>
    {
        private readonly IMyAppService _myAppService;

        public ApplicationService_Validation_Tests()
        {
            _myAppService = GetRequiredService<IMyAppService>();
        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        [Fact]
        public async Task Should_Work_Proper_With_Right_Inputs()
        {
            var output = await _myAppService.MyMethod(new MyMethodInput { MyStringValue = "test" });
            output.Result.ShouldBe(42);
        }

        [Fact]
        public async Task Should_Not_Work_With_Wrong_Inputs()
        {
            await Assert.ThrowsAsync<AbpValidationException>(async () => await _myAppService.MyMethod(new MyMethodInput())); //MyStringValue is not supplied!
            await Assert.ThrowsAsync<AbpValidationException>(async () => await _myAppService.MyMethod(new MyMethodInput { MyStringValue = "a" })); //MyStringValue's min length should be 3!
        }

        [Fact]
        public async Task Should_Work_With_Right_Nesned_Inputs()
        {
            var output = await _myAppService.MyMethod2(new MyMethod2Input
            {
                MyStringValue2 = "test 1",
                Input1 = new MyMethodInput { MyStringValue = "test 2" },
                DateTimeValue = DateTime.Now
            });

            output.Result.ShouldBe(42);
        }

        [Fact]
        public async Task Should_Not_Work_With_Wrong_Nesned_Inputs_1()
        {
            await Assert.ThrowsAsync<AbpValidationException>(async () =>
                await _myAppService.MyMethod2(new MyMethod2Input
                {
                    MyStringValue2 = "test 1",
                    Input1 = new MyMethodInput() //MyStringValue is not set
                }));
        }

        [Fact]
        public async Task Should_Not_Work_With_Wrong_Nesned_Inputs_2()
        {
            await Assert.ThrowsAsync<AbpValidationException>(async () =>
                await _myAppService.MyMethod2(new MyMethod2Input //Input1 is not set
                {
                    MyStringValue2 = "test 1"
                }));
        }

        [Fact]
        public async Task Should_Not_Work_With_Wrong_List_Input_1()
        {
            await Assert.ThrowsAsync<AbpValidationException>(async () =>
                await _myAppService.MyMethod3(
                    new MyMethod3Input
                    {
                        MyStringValue2 = "test 1",
                        ListItems = new List<MyClassInList>
                                    {
                                        new MyClassInList {ValueInList = null}
                                    }
                    }));
        }

        [Fact]
        public async Task Should_Not_Work_With_Wrong_Array_Input_1()
        {
            await Assert.ThrowsAsync<AbpValidationException>(async () =>
                await _myAppService.MyMethod3(
                    new MyMethod3Input
                    {
                        MyStringValue2 = "test 1",
                        ArrayItems = new[]
                                     {
                                         new MyClassInList {ValueInList = null}
                                     }
                    }));
        }

        [Fact]
        public async Task Should_Not_Work_If_Array_Is_Null()
        {
            await Assert.ThrowsAsync<AbpValidationException>(async () =>
                await _myAppService.MyMethod4(new MyMethod4Input()) //ArrayItems is null!
            );
        }

        [Fact]
        public async Task Should_Work_If_Array_Is_Null_But_DisabledValidation_For_Method()
        {
            await _myAppService.MyMethod4_2(new MyMethod4Input());
        }

        [Fact]
        public async Task Should_Work_If_Array_Is_Null_But_DisabledValidation_For_Property()
        {
            await _myAppService.MyMethod5(new MyMethod5Input());
        }

        [Fact]
        public async Task Should_Use_IValidatableObject()
        {
            await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                await _myAppService.MyMethod6(new MyMethod6Input
                {
                    MyStringValue = "test value" //MyIntValue has not set!
                });
            });
        }

        //TODO: Create a Volo.Abp.Ddd.Application.Contracts.Tests project and move this to there and remove Volo.Abp.Ddd.Application.Contracts dependency from this project.
        [Fact]
        public async Task LimitedResultRequestDto_Should_Throw_Exception_For_Requests_More_Than_MaxMaxResultCount()
        {
            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                await _myAppService.MyMethodWithLimitedResult(new LimitedResultRequestDto
                {
                    MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount + 1
                });
            });

            exception.ValidationErrors.ShouldContain(e => e.MemberNames.Contains(nameof(LimitedResultRequestDto.MaxResultCount)));
        }

        [Fact]
        public async Task LimitedResultRequestDto_Should_Be_Valid_For_Requests_Less_Than_MaxMaxResultCount()
        {
            await _myAppService.MyMethodWithLimitedResult(new LimitedResultRequestDto
            {
                MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount - 1
            });
        }

        [Fact]
        public async Task Should_Stop_Recursive_Validation_In_A_Constant_Depth()
        {
            (await _myAppService.MyMethod8(new MyClassWithRecursiveReference { Value = "42" })).Result.ShouldBe(42);
        }

        [Fact]
        public async Task Should_Allow_Null_For_Nullable_Enums()
        {
            await _myAppService.MyMethodWithNullableEnum(null);
        }

        [Fact]
        public void Should_Validate_Emails()
        {
            //Valid
            ValidationHelper.IsValidEmailAddress("john.doe@domain.com").ShouldBe(true);
            ValidationHelper.IsValidEmailAddress("john-doe1@domain.co").ShouldBe(true);
            ValidationHelper.IsValidEmailAddress("ip@1.2.3.123").ShouldBe(true);
            ValidationHelper.IsValidEmailAddress("pharaoh@egyptian.museum").ShouldBe(true);
            ValidationHelper.IsValidEmailAddress("john.doe+regexbuddy@gmail.com").ShouldBe(true);
            ValidationHelper.IsValidEmailAddress("Mike.O'Dell@ireland.com").ShouldBe(true);
            ValidationHelper.IsValidEmailAddress("admin@localhost").ShouldBe(true);
            ValidationHelper.IsValidEmailAddress("j@h.c").ShouldBe(true);

            //Invalid
            ValidationHelper.IsValidEmailAddress("not.a.valid.email").ShouldBe(false);
            ValidationHelper.IsValidEmailAddress("john@aol...com").ShouldBe(false);
            ValidationHelper.IsValidEmailAddress("john@aol@domain.com").ShouldBe(false);
            ValidationHelper.IsValidEmailAddress("jack@domain.").ShouldBe(false);
        }

        [DependsOn(typeof(AbpAutofacModule))]
        [DependsOn(typeof(AbpValidationModule))]
        public class TestModule : AbpModule
        {
            public override void PreConfigureServices(ServiceConfigurationContext context)
            {
                context.Services.OnRegistred(onServiceRegistredContext =>
                {
                    if (typeof(IMyAppService).IsAssignableFrom(onServiceRegistredContext.ImplementationType) &&
                        !DynamicProxyIgnoreTypes.Contains(onServiceRegistredContext.ImplementationType))
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
            Task<MyMethodOutput> MyMethod(MyMethodInput input);
            Task<MyMethodOutput> MyMethod2(MyMethod2Input input);
            Task<MyMethodOutput> MyMethod3(MyMethod3Input input);
            Task<MyMethodOutput> MyMethod4(MyMethod4Input input);
            Task<MyMethodOutput> MyMethod4_2(MyMethod4Input input);
            Task<MyMethodOutput> MyMethod5(MyMethod5Input input);
            Task<MyMethodOutput> MyMethod6(MyMethod6Input input);
            Task<MyMethodOutput> MyMethod8(MyClassWithRecursiveReference input);
            Task MyMethodWithNullableEnum(MyEnum? value);
            Task MyMethodWithLimitedResult(LimitedResultRequestDto input);
        }

        public class MyAppService : IMyAppService, ITransientDependency
        {
            public Task<MyMethodOutput> MyMethod(MyMethodInput input)
            {
                return Task.FromResult(new MyMethodOutput { Result = 42 });
            }

            public Task<MyMethodOutput> MyMethod2(MyMethod2Input input)
            {
                return Task.FromResult(new MyMethodOutput { Result = 42 });
            }

            public Task<MyMethodOutput> MyMethod3(MyMethod3Input input)
            {
                return Task.FromResult(new MyMethodOutput { Result = 42 });
            }

            public Task<MyMethodOutput> MyMethod4(MyMethod4Input input)
            {
                return Task.FromResult(new MyMethodOutput { Result = 42 });
            }

            [DisableValidation]
            public Task<MyMethodOutput> MyMethod4_2(MyMethod4Input input)
            {
                return Task.FromResult(new MyMethodOutput { Result = 42 });
            }

            public Task<MyMethodOutput> MyMethod5(MyMethod5Input input)
            {
                return Task.FromResult(new MyMethodOutput { Result = 42 });
            }

            public Task<MyMethodOutput> MyMethod6(MyMethod6Input input)
            {
                return Task.FromResult(new MyMethodOutput { Result = 42 });
            }

            public Task<MyMethodOutput> MyMethod8(MyClassWithRecursiveReference input)
            {
                return Task.FromResult(new MyMethodOutput { Result = 42 });
            }

            public Task MyMethodWithLimitedResult(LimitedResultRequestDto input)
            {
                return Task.CompletedTask;
            }

            public Task MyMethodWithNullableEnum(MyEnum? value)
            {
                return Task.CompletedTask;
            }
        }

        public class MyMethodInput
        {
            [Required]
            [MinLength(3)]
            public string MyStringValue { get; set; }
        }

        public class MyMethod2Input
        {
            [Required]
            [MinLength(2)]
            public string MyStringValue2 { get; set; }

            public DateTime DateTimeValue { get; set; }

            [Required]
            public MyMethodInput Input1 { get; set; }
        }

        public class MyMethod3Input
        {
            [Required]
            [MinLength(2)]
            public string MyStringValue2 { get; set; }

            public List<MyClassInList> ListItems { get; set; }

            public MyClassInList[] ArrayItems { get; set; }
        }

        public class MyMethod4Input
        {
            [Required]
            public MyClassInList[] ArrayItems { get; set; }
        }

        public class MyMethod5Input
        {
            [DisableValidation]
            public MyClassInList[] ArrayItems { get; set; }
        }

        public class MyMethod6Input : IValidatableObject
        {
            [Required]
            [MinLength(2)]
            public string MyStringValue { get; set; }

            public int MyIntValue { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (MyIntValue < 18)
                {
                    yield return new ValidationResult("MyIntValue must be greather than or equal to 18");
                }
            }
        }

        public class MyClassInList
        {
            [Required]
            [MinLength(3)]
            public string ValueInList { get; set; }
        }

        public class MyMethodOutput
        {
            public int Result { get; set; }
        }

        public class MyClassWithRecursiveReference
        {
            public MyClassWithRecursiveReference Reference { get; }

            [Required]
            public string Value { get; set; }

            public MyClassWithRecursiveReference()
            {
                Reference = this;
            }
        }

        public enum MyEnum
        {
            Value1,
            Value2
        }
    }
}