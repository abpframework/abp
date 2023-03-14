namespace LocalizationKeySynchronizer;

public class ArgumentCountMismatch : AbpAsyncKey
{
    public ArgumentCountMismatch(string key, string reference, int referenceArgumentCount, int argumentCount, string value) : base(key, reference)
    {
        ReferenceArgumentCount = referenceArgumentCount;
        ArgumentCount = argumentCount;
        Value = value;
    }

    public int ReferenceArgumentCount { get; }
    public int ArgumentCount { get;  }
    public string Value { get; }
}