namespace Volo.Abp.Validation.StringValues
{
    public interface ISelectionStringValueItem
    {
        string Value { get; set; }

        LocalizableStringInfo DisplayText { get; set; }
    }
}