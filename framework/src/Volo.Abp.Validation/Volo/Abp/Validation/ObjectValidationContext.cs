using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Volo.Abp.Validation;

public class ObjectValidationContext
{
    [NotNull]
    public object ValidatingObject { get; }

    public List<ValidationResult> Errors { get; }

    public ObjectValidationContext([NotNull] object validatingObject)
    {
        ValidatingObject = Check.NotNull(validatingObject, nameof(validatingObject));
        Errors = new List<ValidationResult>();
    }
}
