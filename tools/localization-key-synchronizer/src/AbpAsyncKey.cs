namespace LocalizationKeySynchronizer;

public class AbpAsyncKey
{
    public string NewValue = string.Empty;

    public AbpAsyncKey(string key, string reference)
    {
        Key = key;
        Reference = reference;
    }

    public virtual string Type => GetType().Name;
    public string Key { get; set; }
    public string Reference { get; set; }
}