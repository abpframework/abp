namespace Volo.Abp.Kafka
{
    public interface IKafkaMessageConsumerFactory
    {
        /// <summary>
        /// Creates a new <see cref="IKafkaMessageConsumer"/>.
        /// Avoid to create too many consumers since they are
        /// not disposed until end of the application.
        /// </summary>
        /// <param name="topicName"></param>
        /// <param name="deadLetterTopicName"></param>
        /// <param name="groupId"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        IKafkaMessageConsumer Create(
            string topicName,
            string deadLetterTopicName,
            string groupId,
            string connectionName = null);
    }
}
