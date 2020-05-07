using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;

namespace Volo.Abp.ObjectExtending
{
    [Serializable]
    public class ExtensibleObject : IHasExtraProperties, IValidatableObject
    {
        public Dictionary<string, object> ExtraProperties { get; protected set; }

        public ExtensibleObject()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ExtensibleObjectValidator.GetValidationErrors(
                this,
                validationContext
            );
        }
    }
}
