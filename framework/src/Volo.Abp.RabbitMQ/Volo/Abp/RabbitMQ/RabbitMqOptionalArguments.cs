using System;
using System.Collections.Generic;

namespace Volo.Abp.RabbitMQ;

public class OptionalArguments
{
    /// <summary>
    ///     Set the queue in classic or quorum or stream
    ///     <para>(Sets the <see href="https://www.rabbitmq.com/queues.html#optional-arguments">"x-queue-type"</see> argument.)</para>
    /// </summary>
    public string? QueueType { get; set; }

    /// <summary>
    ///     How long a queue can be unused for before it is automatically deleted (milliseconds).
    ///     <para>(Sets the <see href="https://rabbitmq.com/ttl.html#queue-ttl">"x-expires"</see> argument.)</para>
    /// </summary>
    public int? Expires { get; set; }

    /// <summary>
    ///     How long a message published to a queue can live before it is discarded (milliseconds).
    ///     <para>(Sets the <see href="https://rabbitmq.com/ttl.html#per-queue-message-ttl">"x-message-ttl"</see> argument.)</para>
    /// </summary>
    public int? MessageTtl { get; set; }

    /// <summary>
    ///     Sets the queue overflow behaviour. This determines what happens to messages when the maximum length of a queue is
    ///     reached. Valid values are drop-head, reject-publish or reject-publish-dlx. The quorum queue type only supports
    ///     drop-head and reject-publish.
    ///     <para>(Sets the <see href="https://www.rabbitmq.com/maxlength.html#overflow-behaviour">"x-overflow"</see> argument.)</para>
    /// </summary>
    public string? Overflow { get; set; }

    /// <summary>
    ///     If set, makes sure only one consumer at a time consumes from the queue and fails over to another registered
    ///     consumer in case the active one is cancelled or dies.
    ///     <para>(Sets the <see href="https://rabbitmq.com/consumers.html#single-active-consumer">"x-single-active-consumer"</see> argument.)</para>
    /// </summary>
    public bool? SingleActiveConsumer { get; set; }

    /// <summary>
    ///     Optional name of an exchange to which messages will be republished if they are rejected or expire.
    ///     <para>(Sets the <see href="https://rabbitmq.com/dlx.html">"x-dead-letter-exchange"</see> argument.)</para>
    /// </summary>
    public string? DeadLetterExchange { get; set; }

    /// <summary>
    ///     Optional replacement routing key to use when a message is dead-lettered. If this is not set, the message's original
    ///     routing key will be used.
    ///     <para>(Sets the <see href="https://rabbitmq.com/dlx.html">"x-dead-letter-routing-key"</see> argument.)</para>
    /// </summary>
    public string? DeadLetterRoutingKey { get; set; }

    /// <summary>
    ///     How many (ready) messages a queue can contain before it starts to drop them from its head.
    ///     <para>(Sets the <see href="https://rabbitmq.com/maxlength.html">"x-max-length"</see> argument.)</para>
    /// </summary>
    public int? MaxLength { get; set; }

    /// <summary>
    ///     Total body size for ready messages a queue can contain before it starts to drop them from its head.
    ///     <para>(Sets the <see href="https://rabbitmq.com/maxlength.html">"x-max-length-bytes"</see> argument.)</para>
    /// </summary>
    public int? MaxLengthBytes { get; set; }

    /// <summary>
    ///     Maximum number of priority levels for the queue to support; if not set, the queue will not support message
    ///     priorities.
    ///     <para>(Sets the <see href="https://rabbitmq.com/priority.html">"x-max-priority"</see> argument.)</para>
    /// </summary>
    public int? MaxPriority { get; set; }

    /// <summary>
    ///     Set the queue version. Defaults to version 1.
    ///     Version 1 has a journal-based index that embeds small messages.
    ///     Version 2 has a different index which improves memory usage and performance in many scenarios, as well as a
    ///     per-queue store for messages that were previously embedded.
    ///     <para>(Sets the <see href="https://rabbitmq.com/persistence-conf.html#queue-version">"x-queue-version"</see> argument.)</para>
    /// </summary>
    public int? QueueVersion { get; set; }

    /// <summary>
    ///     Set the queue into master location mode, determining the rule by which the queue master is located when declared on
    ///     a cluster of nodes.
    ///     <para>(Sets the <see href="https://www.rabbitmq.com/ha.html#queue-leader-location">"x-queue-master-locator"</see> argument.)</para>
    /// </summary>
    public string? QueueMasterLocator { get; set; }

    /// <summary>
    ///     The number of allowed unsuccessful delivery attempts. Once a message has been delivered unsuccessfully more than
    ///     this many times it will be dropped or dead-lettered, depending on the queue configuration.
    ///     <para>(Sets the <see href="https://www.rabbitmq.com/quorum-queues.html#poison-message-handling">"x-delivery-limit"</see> argument.)</para>
    /// </summary>
    public int? DeliveryLimit { get; set; }

    /// <summary>
    ///     Set the queue initial cluster size.
    ///     <para>(Sets the <see href="https://www.rabbitmq.com/quorum-queues.html#replication-factor">"x-quorum-initial-group-size"</see> argument.)</para>
    /// </summary>
    public int? QuorumInitialGroupSize { get; set; }

    /// <summary>
    ///     Valid values are at-most-once or at-least-once. It defaults to at-most-once. This setting is understood only by
    ///     quorum queues. If at-least-once is set, Overflow behaviour must be set to reject-publish. Otherwise, dead letter
    ///     strategy will fall back to at-most-once.
    ///     <para>(Sets the <see href="https://www.rabbitmq.com/quorum-queues.html#dead-lettering">"x-dead-letter-strategy"</see> argument.)</para>
    /// </summary>
    public string? DeadLetterStrategy { get; set; }

    /// <summary>
    ///     Set the rule by which the queue leader is located when declared on a cluster of nodes. Valid values are
    ///     client-local (default) and balanced.
    ///     <para>(Sets the <see href="https://www.rabbitmq.com/streams.html#leader-election">"x-queue-leader-locator"</see> argument.)</para>
    /// </summary>
    public string? QueueLeaderLocator { get; set; }

    /// <summary>
    ///     Sets the data retention for stream queues in time units
    ///     (Y=Years, M=Months, D=Days, h=hours, m=minutes, s=seconds).
    ///     E.g. "1h" configures the stream to only keep the last 1 hour of received messages.
    ///     <para>(Sets the <see href="https://www.rabbitmq.com/streams.html#declaring">"x-max-age"</see> argument.)</para>
    /// </summary>
    public string? MaxAge { get; set; }

    /// <summary>
    ///     Total segment size for stream segments on disk.
    ///     <para>(Sets the <see href="https://www.rabbitmq.com/streams.html#declaring">"x-stream-max-segment-size-bytes"</see> argument.)</para>
    /// </summary>
    public int? StreamMaxSegmentSizeInBytes { get; set; }

    /// <summary>
    ///     Set the queue initial cluster size.
    ///     <para>(Sets the <see href="https://www.rabbitmq.com/streams.html#replication-factor">"x-initial-cluster-size"</see> argument.)</para>
    /// </summary>
    public int? InitialClusterSize { get; set; }

    /// <summary>
    ///     If messages to this exchange cannot otherwise be routed, send them to the alternate exchange named here.
    ///     <para>(Sets the <see href="https://rabbitmq.com/ae.html">"alternate-exchange"</see> argument.)</para>
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
