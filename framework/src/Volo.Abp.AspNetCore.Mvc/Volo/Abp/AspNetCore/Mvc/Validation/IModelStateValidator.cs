using Microsoft.AspNetCore.Mvc.ModelBinding;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.Validation;

public interface IModelStateValidator
{
    void Validate(ModelStateDictionary modelState);

    void AddErrors(IAbpValidationResult validationResult, ModelStateDictionary modelState);
}
