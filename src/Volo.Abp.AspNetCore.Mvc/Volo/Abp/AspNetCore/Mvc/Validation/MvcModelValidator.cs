using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    [ExposeServices(typeof(IMvcModelObjectValidator))]
    public class MvcModelValidator : ObjectValidator, IMvcModelObjectValidator
    {
        public MvcModelValidator(IOptions<AbpValidationOptions> options) 
            : base(options, NullDataAnnotationValidator.Instance)
        {
        }
    }
}