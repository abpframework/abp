using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Validation
{
    public interface IAbpValidationResult
    {
        List<ValidationResult> Errors { get; }
    }
}