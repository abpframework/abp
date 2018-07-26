using RabbitMQ.Client;

namespace Volo.Abp.RabbitMQ
{
    public class ChannelPoolItem
    {
        public IModel Channel { get; set; }

        public bool IsInUse
        {
            get => _isInUse;
            set => _isInUse = value;
        }
        private volatile bool _isInUse;
    }
}