using System.Runtime.Serialization;
using Volo.Abp;

namespace Volo.CmsKit.MediaDescriptors
{
    public class EntityCantHaveMediaException : BusinessException
    {
        public EntityCantHaveMediaException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {
        }

        public EntityCantHaveMediaException(string entityType) 
        {
            EntityType = entityType;
            Code = CmsKitErrorCodes.MediaDescriptors.EntityTypeDoesntExist;
            WithData(nameof(entityType), EntityType);
        }

        public string EntityType { get; }
    }
}
