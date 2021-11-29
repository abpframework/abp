using System;

namespace Volo.Abp.Application.Dtos
{
    [Serializable]
    public abstract class EntityDto : IEntityDto //TODO: Consider to delete this class
    {
        public override string ToString()
        {
            return $"[DTO: {GetType().Name}]";
        }
    }

    [Serializable]
    public abstract class EntityDto<TKey> : EntityDto, IEntityDto<TKey>
    {
        /// <summary>
        /// Id of the entity.
        /// </summary>
        public TKey Id { get; set; }

        public override string ToString()
        {
            return $"[DTO: {GetType().Name}] Id = {Id}";
        }
    }
}