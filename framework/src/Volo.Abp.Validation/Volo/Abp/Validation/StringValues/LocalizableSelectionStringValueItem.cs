namespace Volo.Abp.Validation.StringValues;

public class LocalizableSelectionStringValueItem : ISelectionStringValueItem
{
    public string Value { get; set; }

    public LocalizableStringInfo DisplayText { get; set; }
}
