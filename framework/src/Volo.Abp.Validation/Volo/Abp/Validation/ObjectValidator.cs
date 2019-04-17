using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Validation
{
    public class ObjectValidator : IObjectValidator, ITransientDependency
    {
        public virtual void Validate(object validatingObject, string name = null, bool allowNull = false)
        {
            var errors = GetErrors(validatingObject, name, allowNull);

            if (errors.Any())
            {
                //TODO: How to localize messages?
                throw new AbpValidationException(
                    "Object state is not valid! See ValidationErrors for details.",
                    errors
                );
            }
        }

        public virtual List<ValidationResult> GetErrors(object validatingObject, string name = null,
            bool allowNull = false)
        {
            var errors = new List<ValidationResult>();

            if (validatingObject == null && !allowNull)
            {
                //TODO: How to localize messages?
                errors.Add(
                    name == null
                        ? new ValidationResult("Given object is null!")
                        : new ValidationResult(name + " is null!", new[] {name})
                );

                return errors;
            }

            return errors;
        }
    }
}