using System;

namespace Volo.Abp.RabbitMQ;

public abstract class AbpRabbitMqExchangeAndQueueBaseOptions
{
    public string? ConnectionName { get; set; }

    public string? ClientName { get; set; }

    public bool QueueDurable { get; set; }

    public bool QueueExclusive { get; set; } = true;
    
    public bool QueueAutoDelete { get; set; } = true;
    
    public OptionalArguments? QueueArguments { get; set; }

    public string? ExchangeName { get; set; }

    private string? _exchangeType;

    public string ExchangeType {
        get {
            return _exchangeType.IsNullOrWhiteSpace() ? RabbitMqConsts.ExchangeTypes.Direct : _exchangeType!;
        }
        set {
            _exchangeType = value;
        }
    }

    public bool ExchangeDurable { get; set; }

    public bool ExchangeAutoDelete { get; set; }
    
    public OptionalArguments? ExchangeArguments { get; set; }
    
    public ushort? PrefetchCount { get; set; }
}
