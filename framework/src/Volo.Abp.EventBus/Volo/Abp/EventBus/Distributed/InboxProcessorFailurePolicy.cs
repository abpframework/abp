namespace Volo.Abp.EventBus.Distributed;

public enum InboxProcessorFailurePolicy
{
    /// <summary>
    /// Default behavior, retry the following event in next period time.
    /// </summary>
    Retry,

    /// <summary>
    /// Skip the failed event and retry it after a delay. 
    /// The delay doubles with each retry, starting from the configured InboxProcessorRetryBackoffFactor 
    /// (e.g., 10, 20, 40, 80 seconds, etc.). 
    /// The event is discarded if it still fails after reaching the maximum retry count.
    /// </summary>
    RetryLater,

    /// <summary>
    /// Skip the event and do not retry it.
    /// </summary>
    Discard,
}
