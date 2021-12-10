using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.Validation;

public class ModelStateValidator : IModelStateValidator, ITransientDependency
{
    public virtual void Validate(ModelStateDictionary modelState)
    {
        var validationResult = new AbpValidationResult();

        AddErrors(validationResult, modelState);

        if (validationResult.Errors.Any())
        {
            throw new AbpValidationException(
                "ModelState is not valid! See ValidationErrors for details.",
                validationResult.Errors
            );
        }
    }

    public virtual void AddErrors(IAbpValidationResult validationResult, ModelStateDictionary modelState)
    {
        if (modelState.IsValid)
        {
            return;
        }

        foreach (var state in modelState)
        {
            foreach (var error in state.Value.Errors)
            {
                validationResult.Errors.Add(new ValidationResult(error.ErrorMessage, new[] { state.Key }));
            }
        }
    }
}
