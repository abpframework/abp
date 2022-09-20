using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using Volo.Abp.RabbitMQ;

namespace Volo.Abp.BackgroundJobs.RabbitMQ;

public class JobQueueConfiguration : QueueDeclareConfiguration
{
    public Type JobArgsType { get; }

    public string ConnectionName { get; set; }

    public string DelayedQueueName { get; set; }

    public JobQueueConfiguration(
        Type jobArgsType,
        string queueName,
        string delayedQueueName,
        string connectionName = null,
        bool durable = true,
        bool exclusive = false,
        bool autoDelete = false)
        : base(
            queueName,
            durable,
            exclusive,
            autoDelete)
    {
        JobArgsType = jobArgsType;
        ConnectionName = connectionName;
        DelayedQueueName = delayedQueueName;
    }

    public virtual QueueDeclareOk DeclareDelayed(IModel channel)
    {
        var delayedArguments = new Dictionary<string, object>(Arguments)
        {
            ["x-dead-letter-routing-key"] = QueueName,
            ["x-dead-letter-exchange"] = string.Empty
        };

        return channel.QueueDeclare(
            queue: DelayedQueueName,
            durable: Durable,
            exclusive: Exclusive,
            autoDelete: AutoDelete,
            arguments: delayedArguments
        );
    }
}
