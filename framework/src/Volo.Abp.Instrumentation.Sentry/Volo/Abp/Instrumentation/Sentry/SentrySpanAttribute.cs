using JetBrains.Annotations;

namespace Volo.Abp.Instrumentation.Sentry;

/// <summary>
/// Used to indicate that declaring method (or all methods of the class) should get child Sentry span.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class SentrySpanAttribute : Attribute
{
    /// <summary>
    /// Sentry span operation name. 
    /// Uses ClassName.MethodName if not supplied.
    /// </summary>
    public string? Operation { get; set; }
    
    /// <summary>
    /// Sentry span description.
    /// Sentry uses SpanId if not supplied.
    /// Default: null
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Used to prevent starting a Sentry span for the method.
    /// Default: false.
    /// </summary>
    public bool IsDisabled { get; set; }


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
    
    public SentrySpanAttribute(string? operation = null, string? description = null)
    {
        Operation = operation;
        Description = description;
    }
}