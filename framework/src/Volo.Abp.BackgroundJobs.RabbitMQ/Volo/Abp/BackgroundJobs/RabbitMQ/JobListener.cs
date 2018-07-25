using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Volo.Abp.RabbitMQ;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public class JobListener<TJob> : IJobListener
    {
        protected string JobName { get; }
        protected IBackgroundJobExecuter JobExecuter { get; }
        protected IChannelPool ChannelPool { get; }
        protected IRabbitMqSerializer Serializer { get; }
        protected Type ArgsType { get; }

        protected IChannelAccessor ChannelAccessor { get; private set; }
        protected EventingBasicConsumer Consumer { get; private set; }

        public JobListener(
            IChannelPool channelPool, 
            IBackgroundJobExecuter jobExecuter, 
            IRabbitMqSerializer serializer)
        {
            ChannelPool = channelPool;
            JobExecuter = jobExecuter;
            Serializer = serializer;
            ArgsType = BackgroundJobArgsHelper.GetJobArgsType(typeof(TJob));
            JobName = BackgroundJobNameAttribute.GetName(ArgsType);
        }

        public void Start()
        {
            var queueName = "BackgroundJobs." + JobName; //TODO: Make prefix optional

            ChannelAccessor = ChannelPool.Acquire(queueName);

            //TODO: How to ensure that queue is created!

            Consumer = new EventingBasicConsumer(ChannelAccessor.Channel);
            Consumer.Received += MessageReceived;

            //TODO: What BasicConsume returns?
            ChannelAccessor.Channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: Consumer
            );
        }

        private void MessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var context = new JobExecutionContext(typeof(TJob), Serializer.Deserialize(e.Body, ArgsType));
            JobExecuter.Execute(context);
            //TODO: How to ACK on success or Reject on failure?
        }

        public void Stop()
        {
            ChannelAccessor.Dispose();
        }
    }
}