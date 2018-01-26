namespace Volo.Abp.Application.Dtos
{
    public class EntityDto : IEntityDto //TODO: Consider to delete this class
    {
        public override string ToString()
        {
            return $"[DTO: {GetType().Name}]";
        }
    }

    public class EntityDto<TKey> : EntityDto, IEntityDto<TKey>
    {
        /// <summary>
        /// Id of the entity.
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// Creates a new <see cref="EntityDto{TKey}"/> object.
        /// </summary>
        public EntityDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityDto{TKey}"/> object.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public EntityDto(TKey id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"[DTO: {GetType().Name}] Id = {Id}";
        }
    }
}