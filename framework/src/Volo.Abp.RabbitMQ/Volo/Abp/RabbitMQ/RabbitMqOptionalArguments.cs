using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.RabbitMQ;

public class OptionalArguments
{
    /// <summary>
    ///     Set the queue in classic or quorum or stream
    ///     (Sets the "x-queue-type" argument.)
    /// </summary>
    public string? QueueType { get; set; }

    /// <summary>
    ///     How long a queue can be unused for before it is automatically deleted (milliseconds).
    ///     (Sets the "x-expires" argument.)
    /// </summary>
    public int? Expires { get; set; }

    /// <summary>
    ///     How long a message published to a queue can live before it is discarded (milliseconds).
    ///     (Sets the "x-message-ttl" argument.)
    /// </summary>
    public int? MessageTtl { get; set; }

    /// <summary>
    ///     Sets the queue overflow behaviour. This determines what happens to messages when the maximum length of a queue is
    ///     reached. Valid values are drop-head, reject-publish or reject-publish-dlx. The quorum queue type only supports
    ///     drop-head and reject-publish.
    ///     (Sets the "x-overflow" argument.)
    /// </summary>
    public string? Overflow { get; set; }

    /// <summary>
    ///     If set, makes sure only one consumer at a time consumes from the queue and fails over to another registered
    ///     consumer in case the active one is cancelled or dies.
    ///     (Sets the "x-single-active-consumer" argument.)
    /// </summary>
    public bool? SingleActiveConsumer { get; set; }

    /// <summary>
    ///     Optional name of an exchange to which messages will be republished if they are rejected or expire.
    ///     (Sets the "x-dead-letter-exchange" argument.)
    /// </summary>
    public string? DeadLetterExchange { get; set; }

    /// <summary>
    ///     Optional replacement routing key to use when a message is dead-lettered. If this is not set, the message's original
    ///     routing key will be used.
    ///     (Sets the "x-dead-letter-routing-key" argument.)
    /// </summary>
    public string? DeadLetterRoutingKey { get; set; }

    /// <summary>
    ///     How many (ready) messages a queue can contain before it starts to drop them from its head.
    ///     (Sets the "x-max-length" argument.)
    /// </summary>
    public int? MaxLength { get; set; }

    /// <summary>
    ///     Total body size for ready messages a queue can contain before it starts to drop them from its head.
    ///     (Sets the "x-max-length-bytes" argument.)
    /// </summary>
    public int? MaxLengthBytes { get; set; }

    /// <summary>
    ///     Maximum number of priority levels for the queue to support; if not set, the queue will not support message
    ///     priorities.
    ///     (Sets the "x-max-priority" argument.)
    /// </summary>
    public int? MaxPriority { get; set; }

    /// <summary>
    ///     Set the queue version. Defaults to version 1.
    ///     Version 1 has a journal-based index that embeds small messages.
    ///     Version 2 has a different index which improves memory usage and performance in many scenarios, as well as a
    ///     per-queue store for messages that were previously embedded.
    ///     (Sets the "x-queue-version" argument.)
    /// </summary>
    public int? QueueVersion { get; set; }

    /// <summary>
    ///     Set the queue into master location mode, determining the rule by which the queue master is located when declared on
    ///     a cluster of nodes.
    ///     (Sets the "x-queue-master-locator" argument.)
    /// </summary>
    public string? QueueMasterLocator { get; set; }

    /// <summary>
    ///     The number of allowed unsuccessful delivery attempts. Once a message has been delivered unsuccessfully more than
    ///     this many times it will be dropped or dead-lettered, depending on the queue configuration.
    ///     (Sets the "x-delivery-limit" argument.)
    /// </summary>
    public int? DeliveryLimit { get; set; }

    /// <summary>
    ///     Set the queue initial cluster size.
    ///     (Sets the "x-quorum-initial-group-size" argument.)
    /// </summary>
    public int? QuorumInitialGroupSize { get; set; }

    /// <summary>
    ///     Valid values are at-most-once or at-least-once. It defaults to at-most-once. This setting is understood only by
    ///     quorum queues. If at-least-once is set, Overflow behaviour must be set to reject-publish. Otherwise, dead letter
    ///     strategy will fall back to at-most-once.
    ///     (Sets the "x-dead-letter-strategy" argument.)
    /// </summary>
    public string? DeadLetterStrategy { get; set; }

    /// <summary>
    ///     Set the rule by which the queue leader is located when declared on a cluster of nodes. Valid values are
    ///     client-local (default) and balanced.
    ///     (Sets the "x-queue-leader-locator" argument.)
    /// </summary>
    public string? QueueLeaderLocator { get; set; }

    /// <summary>
    ///     Sets the data retention for stream queues in time units
    ///     (Y=Years, M=Months, D=Days, h=hours, m=minutes, s=seconds).
    ///     E.g. "1h" configures the stream to only keep the last 1 hour of received messages.
    ///     (Sets the x-max-age argument.)
    /// </summary>
    public string? MaxAge { get; set; }

    /// <summary>
    ///     Total segment size for stream segments on disk.
    ///     (Sets the x-stream-max-segment-size-bytes argument.)
    /// </summary>
    public int? StreamMaxSegmentSizeInBytes { get; set; }

    /// <summary>
    ///     Set the queue initial cluster size.
    ///     (Sets the x-initial-cluster-size argument.)
    /// </summary>
    public int? InitialClusterSize { get; set; }

    /// <summary>
    ///     If messages to this exchange cannot otherwise be routed, send them to the alternate exchange named here.
    ///     (Sets the "alternate-exchange" argument.)
    /// </summary>
    public string? AlternateExchange { get; set; }

    public static implicit operator Dictionary<string, object>?(OptionalArguments? args)
    {
        if (args is null) return null;

        var items = new Dictionary<string, object>();
        if (!args.QueueType.IsNullOrWhiteSpace()) items.Add("x-queue-type", args.QueueType!);
        if (args.Expires.HasValue) items.Add("x-expires", args.Expires.Value);
        if (args.MessageTtl.HasValue) items.Add("x-message-ttl", args.MessageTtl.Value);
        if (!args.Overflow.IsNullOrWhiteSpace()) items.Add("x-overflow", args.Overflow!);
        if (args.SingleActiveConsumer.HasValue) items.Add("x-single-active-consumer", args.SingleActiveConsumer.Value);
        if (!args.DeadLetterExchange.IsNullOrWhiteSpace()) items.Add("x-dead-letter-exchange", args.DeadLetterExchange!);
        if (!args.DeadLetterRoutingKey.IsNullOrWhiteSpace()) items.Add("x-dead-letter-routing-key", args.DeadLetterRoutingKey!);
        if (args.MaxLength.HasValue) items.Add("x-max-length", args.MaxLength.Value);
        if (args.MaxLengthBytes.HasValue) items.Add("x-max-length-bytes", args.MaxLengthBytes.Value);
        if (args.MaxPriority.HasValue) items.Add("x-max-priority", args.MaxPriority.Value);
        if (args.QueueVersion.HasValue) items.Add("x-queue-version", args.QueueVersion.Value);
        if (!args.QueueMasterLocator.IsNullOrWhiteSpace()) items.Add("x-queue-master-locator", args.QueueMasterLocator!);
        if (args.DeliveryLimit.HasValue) items.Add("x-delivery-limit", args.DeliveryLimit.Value);
        if (args.QuorumInitialGroupSize.HasValue) items.Add("x-quorum-initial-group-size", args.QuorumInitialGroupSize.Value);
        if (!args.DeadLetterStrategy.IsNullOrWhiteSpace()) items.Add("x-dead-letter-strategy", args.DeadLetterStrategy!);
        if (!args.QueueLeaderLocator.IsNullOrWhiteSpace()) items.Add("x-queue-leader-locator", args.QueueLeaderLocator!);
        if (!args.MaxAge.IsNullOrWhiteSpace()) items.Add("x-max-age", args.MaxAge!);
        if (args.StreamMaxSegmentSizeInBytes.HasValue) items.Add("x-stream-max-segment-size-bytes", args.StreamMaxSegmentSizeInBytes.Value);
        if (args.InitialClusterSize.HasValue) items.Add("x-initial-cluster-size", args.InitialClusterSize.Value);
        if (!args.AlternateExchange.IsNullOrWhiteSpace()) items.Add("alternate-exchange", args.AlternateExchange!);

        return items.Count > 0 ? items : null;
    }
}
