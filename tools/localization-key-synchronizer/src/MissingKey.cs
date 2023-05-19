namespace LocalizationKeySynchronizer;

public class MissingKey : AbpAsyncKey
{
    public MissingKey(string key, string reference) : base(key, reference)
    {
    }
}