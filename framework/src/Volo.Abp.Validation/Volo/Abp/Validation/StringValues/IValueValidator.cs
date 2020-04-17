using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Validation.StringValues
{
    public interface IValueValidator
    {
        string Name { get; }

        [CanBeNull]
        object this[string key] { get; set; }

        [NotNull]
        IDictionary<string, object> Properties { get; }

        bool IsValid(object value);
    }
}