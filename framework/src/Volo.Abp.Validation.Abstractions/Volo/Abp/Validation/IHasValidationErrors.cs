using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Validation
{
    public interface IHasValidationErrors
    {
        IList<ValidationResult> ValidationErrors { get; }
    }
}