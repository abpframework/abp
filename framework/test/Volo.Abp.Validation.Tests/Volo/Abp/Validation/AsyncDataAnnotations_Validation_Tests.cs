using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Validation;

public class AsyncDataAnnotations_Validation_Tests : AbpIntegratedTest<AsyncDataAnnotations_Validation_Tests.TestModule>
{
    private readonly IObjectValidator _objectValidator;

    public AsyncDataAnnotations_Validation_Tests()
    {
        _objectValidator = GetRequiredService<IObjectValidator>();
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Fact]
    public async Task Should_Run_Async_Validation_Attribute()
    {
        var errors = await _objectValidator.GetErrorsAsync(new AsyncAttributeInput { Name = "taken" });

        errors.ShouldContain(e => e.ErrorMessage == "Name is already taken");
    }

    [Fact]
    public async Task Should_Not_Report_Error_When_Async_Validation_Attribute_Passes()
    {
        var errors = await _objectValidator.GetErrorsAsync(new AsyncAttributeInput { Name = "free" });

        errors.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Run_IAsyncValidatableObject()
    {
        var errors = await _objectValidator.GetErrorsAsync(new AsyncValidatableInput { Value = -1 });

        errors.ShouldContain(e => e.ErrorMessage == "Value must not be negative");
    }

    [Fact]
    public async Task Should_Not_Run_Synchronous_Validate_Of_IAsyncValidatableObject()
    {
        var input = new AsyncValidatableInput { Value = 1 };

        await _objectValidator.ValidateAsync(input);

        /* IAsyncValidatableObject extends IValidatableObject. Running both would validate twice and
         * the synchronous overload is the one such a type is expected to throw from. */
        input.SynchronousValidateCallCount.ShouldBe(0);
    }

    public class AsyncAttributeInput
    {
        [NameNotTaken]
        public string? Name { get; set; }
    }

    public class NameNotTakenAttribute : AsyncValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            throw new AbpException("This attribute only validates asynchronously.");
        }

        protected override async Task<ValidationResult?> IsValidAsync(
            object? value,
            ValidationContext validationContext,
            CancellationToken cancellationToken)
        {
            await Task.Yield();

            return value as string == "taken"
                ? new ValidationResult("Name is already taken", new[] { validationContext.MemberName! })
                : ValidationResult.Success;
        }
    }

    public class AsyncValidatableInput : IAsyncValidatableObject
    {
        public int Value { get; set; }

        public int SynchronousValidateCallCount { get; private set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            SynchronousValidateCallCount++;
            return Enumerable.Empty<ValidationResult>();
        }

        public async IAsyncEnumerable<ValidationResult> ValidateAsync(
            ValidationContext validationContext,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.Yield();

            if (Value < 0)
            {
                yield return new ValidationResult("Value must not be negative", new[] { nameof(Value) });
            }
        }
    }

    [DependsOn(typeof(AbpValidationModule), typeof(AbpAutofacModule))]
    public class TestModule : AbpModule
    {
    }
}
