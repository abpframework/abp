using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Volo.Abp.MultiQueue.Publisher;

namespace Volo.Abp.MultiQueue.Kafka;

public class KafkaQueuePublisher<TOptions> : IQueuePublisher<TOptions> where TOptions : KafkaQueueOptions
{
    private readonly IOptions<TOptions> _options;
    private readonly IProducer<Null, byte[]> _producer;

    protected TOptions Options => _options.Value;
    protected ILogger Logger { get; }

    public KafkaQueuePublisher(ILogger<KafkaQueuePublisher<TOptions>> logger, IOptions<TOptions> options)
    {
        Logger = logger;
        _options = options;
        _producer = GetProducer();
    }

    public async Task PublishAsync(string topic, object data)
    {
        if (data == null) return;

        try
        {
            byte[] pubData = ToByteData(data);
            if (pubData == null) return;

            await _producer.ProduceAsync(topic, new Message<Null, byte[]> { Value = pubData });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Kafka PublishAsync Error");
            throw;
        }
    }

    public async Task BatchPublishAsync(string topic, object[] data)
    {
        try
        {
            foreach (var item in data)
            {
                byte[] pubData = ToByteData(item);
                if (pubData == null) continue;

                _producer.Produce(topic, new Message<Null, byte[]> { Value = pubData });
            }
            _producer.Flush(TimeSpan.FromSeconds(10));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Kafka BatchPublishAsync Error");
            throw;
        }
        await Task.CompletedTask;
    }

    protected virtual byte[] ToByteData(object data)
    {
        byte[] pubData = null;
        if (data is byte[] byteData)
        {
            pubData = byteData;
        }
        else if (data is string strData)
        {
            pubData = System.Text.Encoding.UTF8.GetBytes(strData);
        }
        else
        {
            string jsonOrVal = null;
            try
            {
                jsonOrVal = JsonConvert.SerializeObject(data);
            }
            catch
            {
                jsonOrVal = data.ToString();
            }

            if (jsonOrVal != null)
                pubData = System.Text.Encoding.UTF8.GetBytes(jsonOrVal);
        }

        return pubData;
    }

    protected virtual ProducerConfig GetConfig()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = Options.Address,
            SaslUsername = Options.UserName,
            SaslPassword = Options.Password,
            MessageMaxBytes = Options.MessageMaxBytes,
            SecurityProtocol = Options.SecurityProtocol,
            SaslMechanism = Options.SaslMechanism
        };
        return config;
    }

    protected virtual IProducer<Null, byte[]> GetProducer()
    {
        var config = GetConfig();
        return new ProducerBuilder<Null, byte[]>(config).Build();
    }

    public void Dispose()
    {
        if (_producer != null)
            _producer.Dispose();
    }
}