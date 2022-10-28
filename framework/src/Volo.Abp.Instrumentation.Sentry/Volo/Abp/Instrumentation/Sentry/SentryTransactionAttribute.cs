namespace Volo.Abp.Instrumentation.Sentry.Volo.Abp.Instrumentation.Sentry;

/// <summary>
/// Used to indicate that declaring method (or all methods of the class) could start new transaction or if transaction exist get child Sentry span.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class SentryTransactionAttribute : SentrySpanAttribute
{
    public string Name { get; set; }

    public SentryTransactionAttribute(string name, string? operation = null, string? description = null)
    {
        Name = name;
        Operation = operation;
        Description = description;
    }
}