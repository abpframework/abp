using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Validation
{
    public class AbpValidationResult : IAbpValidationResult
    {
        public List<ValidationResult> Errors { get; }

        public List<IShouldNormalize> ObjectsToBeNormalized { get; }

        public AbpValidationResult()
        {
            Errors = new List<ValidationResult>();
            ObjectsToBeNormalized = new List<IShouldNormalize>();
        }
    }
}