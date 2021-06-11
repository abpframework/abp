using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    [Route("api/validation-test")]
    public class ValidationTestController : AbpController
    {
        [HttpGet]
        [Route("object-result-action")]
        public Task<string> ObjectResultAction(ValidationTest1Model model)
        {
            ModelState.IsValid.ShouldBeTrue(); //AbpValidationFilter throws exception otherwise
            return Task.FromResult(model.Value1);
        }

        [HttpGet]
        [Route("object-result-action-with-custom_validate")]
        public Task<string> ObjectResultActionWithCustomValidate(CustomValidateModel model)
        {
            ModelState.IsValid.ShouldBeTrue(); //AbpValidationFilter throws exception otherwise
            return Task.FromResult(model.Value1);
        }

        [HttpGet]
        [Route("action-result-action")]
        public IActionResult ActionResultAction(ValidationTest1Model model)
        {
            return Content("ModelState.IsValid: " + ModelState.IsValid.ToString().ToLowerInvariant());
        }

        [HttpGet]
        [Route("object-result-action-dynamic-length")]
        public Task<string> ObjectResultActionDynamicLength(ValidationDynamicTestModel model)
        {
            ModelState.IsValid.ShouldBeTrue(); //AbpValidationFilter throws exception otherwise
            return Task.FromResult(model.Value1);
        }

        [HttpGet]
        [Route("disable-validation-object-result-action")]
        [DisableValidation]
        public Task<string> DisableValidationObjectResultAction(ValidationTest1Model model)
        {
            ModelState.IsValid.ShouldBeFalse();
            return Task.FromResult("ModelState.IsValid: " + ModelState.IsValid.ToString().ToLowerInvariant());
        }

        [HttpGet]
        [Route("object-result-action2")]
        public virtual Task<string> ObjectResultAction2(ValidationTest1Model model)
        {
            ModelState.IsValid.ShouldBeTrue(); //AbpValidationFilter throws exception otherwise
            return Task.FromResult(model.Value1);
        }

        [HttpGet]
        [Route("object-result-action3")]
        public virtual Task<string> ObjectResultAction3(ValidationTest1Model model)
        {
            ModelState.IsValid.ShouldBeFalse();
            return Task.FromResult("ModelState.IsValid: " + ModelState.IsValid.ToString().ToLowerInvariant());
        }

        public class ValidationTest1Model
        {
            [Required]
            [StringLength(5, MinimumLength = 2)]
            public string Value1 { get; set; }
        }

        public class ValidationDynamicTestModel
        {
            [DynamicStringLength(typeof(Consts), nameof(Consts.MaxValue1Length), nameof(Consts.MinValue1Length))]
            public string Value1 { get; set; }

            [DynamicMaxLength(typeof(Consts), nameof(Consts.MaxValue2Length))]
            public string Value2 { get; set; }

            [DynamicMaxLength(typeof(Consts), nameof(Consts.MaxValue3Length))]
            public int[] Value3 { get; set; }

            [DynamicRange(typeof(Consts), typeof(int), nameof(Consts.MinValue1), nameof(Consts.MaxValue1))]
            public int Value4 { get; set; }

            [DynamicRange(typeof(Consts), typeof(double), nameof(Consts.MinValue2), nameof(Consts.MaxValue2))]
            public double Value5 { get; set; }

            [DynamicRange(typeof(Consts), typeof(DateTime), nameof(Consts.MinValue3), nameof(Consts.MaxValue3))]
            public DateTime Value6 { get; set; }

            public static class Consts
            {
                public static int MinValue1Length { get; set; } = 2;
                public static int MaxValue1Length { get; set; } = 7;

                public static int MaxValue2Length { get; set; } = 4;

                public static int MaxValue3Length { get; set; } = 2;


                public static int MinValue1 { get; set; } = 1;

                public static int MaxValue1 { get; set; } = 5;


                public static double MinValue2 { get; set; } = 1.2;

                public static double MaxValue2 { get; set; } = 7.2;

                public static string MinValue3 { get; set; } = "1/2/2004";

                public static string MaxValue3 { get; set; } = "3/4/2004";
            }
        }

        public class CustomValidateModel : IValidatableObject
        {
            public string Value1 { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (Value1 != "hello")
                {
                    yield return new ValidationResult("Value1 should be hello");
                }
            }
        }
    }

    [DisableValidation]
    [Route("api/sub1-validation-test")]
    public class Sub1ValidationTestController : ValidationTestController
    {

    }

    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ValidationTestController))]
    public class Sub2ValidationTestController : ValidationTestController
    {
        [DisableValidation]
        public override Task<string> ObjectResultAction2(ValidationTest1Model model)
        {
            ModelState.IsValid.ShouldBeFalse();
            return Task.FromResult("ModelState.IsValid: " + ModelState.IsValid.ToString().ToLowerInvariant());
        }
    }
}
