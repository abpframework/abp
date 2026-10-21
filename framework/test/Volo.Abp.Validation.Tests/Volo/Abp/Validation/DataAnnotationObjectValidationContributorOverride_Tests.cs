using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Autofac;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Validation;

public abstract class DataAnnotationObjectValidationContributorOverride_TestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IObjectValidator ObjectValidator { get; }

    protected DataAnnotationObjectValidationContributorOverride_TestBase()
    {
        ObjectValidator = GetRequiredService<IObjectValidator>();
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected static void UseContributor<TContributor>(ServiceConfigurationContext context)
        where TContributor : DataAnnotationObjectValidationContributor
    {
        context.Services.AddTransient<TContributor>();
        context.Services.Configure<AbpValidationOptions>(options =>
        {
            options.ObjectValidationContributors.Remove<DataAnnotationObjectValidationContributor>();
            options.ObjectValidationContributors.AddIfNotContains(typeof(TContributor));
        });
    }
}

public class ContributorOverrideInput
{
    [Required]
    public string? Name { get; set; }

    [CodeNotTaken]
    public string? Code { get; set; }
}

public class CodeNotTakenAttribute : AsyncValidationAttribute
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
            ? new ValidationResult("Code is already taken", new[] { validationContext.MemberName! })
            : ValidationResult.Success;
    }
}

public class DataAnnotationObjectValidationContributorPropertyOverride_Tests
    : DataAnnotationObjectValidationContributorOverride_TestBase<DataAnnotationObjectValidationContributorPropertyOverride_Tests.TestModule>
{
    [Fact]
    public async Task Should_Call_An_AddPropertyErrors_Override()
    {
        var errors = await ObjectValidator.GetErrorsAsync(new ContributorOverrideInput { Name = "forbidden" });

        errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe("Name is forbidden");
    }

    [Fact]
    public async Task Should_Still_Validate_An_Async_Attribute()
    {
        var errors = await ObjectValidator.GetErrorsAsync(new ContributorOverrideInput { Code = "taken" });

        errors.Select(e => e.ErrorMessage).ShouldBe(new[] { "The Name field is required.", "Code is already taken" }, ignoreOrder: true);
    }

    [DisableConventionalRegistration]
    public class PropertyOverridingContributor : DataAnnotationObjectValidationContributor
    {
        public PropertyOverridingContributor(IOptions<AbpValidationOptions> options, IServiceProvider serviceProvider)
            : base(options, serviceProvider)
        {
        }

        protected override void AddPropertyErrors(object validatingObject, PropertyDescriptor property, List<ValidationResult> errors)
        {
            base.AddPropertyErrors(validatingObject, property, errors);

            if (property.Name == nameof(ContributorOverrideInput.Name) && property.GetValue(validatingObject) as string == "forbidden")
            {
                errors.Add(new ValidationResult("Name is forbidden", new[] { property.Name }));
            }
        }
    }

    [DependsOn(typeof(AbpValidationModule), typeof(AbpAutofacModule))]
    public class TestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            UseContributor<PropertyOverridingContributor>(context);
        }
    }
}

public class DataAnnotationObjectValidationContributorWalkOverride_Tests
    : DataAnnotationObjectValidationContributorOverride_TestBase<DataAnnotationObjectValidationContributorWalkOverride_Tests.TestModule>
{
    [Fact]
    public async Task Should_Call_A_ValidateObjectRecursively_Override()
    {
        var errors = await ObjectValidator.GetErrorsAsync(new ContributorOverrideInput { Name = "valid" });

        errors.ShouldHaveSingleItem().ErrorMessage.ShouldBe("Checked by the override");
    }

    [DisableConventionalRegistration]
    public class WalkOverridingContributor : DataAnnotationObjectValidationContributor
    {
        public WalkOverridingContributor(IOptions<AbpValidationOptions> options, IServiceProvider serviceProvider)
            : base(options, serviceProvider)
        {
        }

        protected override void ValidateObjectRecursively(List<ValidationResult> errors, object? validatingObject, int currentDepth)
        {
            if (currentDepth == 1)
            {
                errors.Add(new ValidationResult("Checked by the override"));
            }
        }
    }

    [DependsOn(typeof(AbpValidationModule), typeof(AbpAutofacModule))]
    public class TestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            UseContributor<WalkOverridingContributor>(context);
        }
    }
}
