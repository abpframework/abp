using System.Collections.Generic;

namespace Volo.Abp.Validation.StringValues;

public interface ISelectionStringValueItemSource
{
    ICollection<ISelectionStringValueItem> Items { get; }
}
