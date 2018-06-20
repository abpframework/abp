using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Validation
{
    public interface IDataAnnotationValidator
    {
        void Validate(object validatingObject);

        List<ValidationResult> GetErrors(object validatingObject);
    }
}