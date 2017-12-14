using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    [ExposeServices(typeof(IMvcModelObjectValidator))]
    public class MvcModelObjectValidator : ObjectValidator, IMvcModelObjectValidator
    {
        public MvcModelObjectValidator(IOptions<AbpValidationOptions> options) 
            : base(options, NullDataAnnotationValidator.Instance)
        {
        }
    }
}