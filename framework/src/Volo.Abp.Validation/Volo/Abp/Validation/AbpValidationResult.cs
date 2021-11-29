using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Validation
{
    public class AbpValidationResult : IAbpValidationResult
    {
        public List<ValidationResult> Errors { get; }

        public AbpValidationResult()
        {
            Errors = new List<ValidationResult>();
        }
    }
}