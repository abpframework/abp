using JetBrains.Annotations;

namespace Volo.Abp.Instrumentation.Sentry.Volo.Abp.Instrumentation.Sentry;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class SentrySpanAttribute : Attribute
{
    public bool IsDisabled { get; set; }
    public string? Operation { get; set; }
    public string? Description { get; set; }


    public SentrySpanAttribute()
    {
        
    }
    
    public SentrySpanAttribute(bool isDisabled)
    {
        IsDisabled = isDisabled;
    }
    
    public SentrySpanAttribute(string operation)
    {
        Operation = operation;
    }
    
    public SentrySpanAttribute(string operation, string description)
    {
        Operation = operation;
        Description = description;
    }
}