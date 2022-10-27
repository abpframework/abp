namespace Volo.Abp.Instrumentation.Sentry.Volo.Abp.Instrumentation.Sentry;

public class SentrySpanAttribute : Attribute
{
    public SentrySpanAttribute()
    {
        
    }
    
    public bool IsDisabled { get; set; }
}