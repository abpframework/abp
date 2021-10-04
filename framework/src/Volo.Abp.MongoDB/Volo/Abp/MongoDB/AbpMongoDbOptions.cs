using Volo.Abp.Timing;

namespace Volo.Abp.MongoDB
{
    public class AbpMongoDbOptions
    {
        /// <summary>
        /// Serializer the datetime based on <see cref="AbpClockOptions.Kind"/> in MongoDb.
        /// Default: true.
        /// </summary>
        public bool UseAbpClockHandleDateTime { get; set; }

        public AbpMongoDbOptions()
        {
            UseAbpClockHandleDateTime = true;
        }
    }
}
