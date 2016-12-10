namespace Volo.Abp.Application.Services.Dtos
{
    /// <summary>
    /// Implements common properties for entity based DTOs.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key</typeparam>
    public class EntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Id of the entity.
        /// </summary>
        public TPrimaryKey Id { get; set; }

        /// <summary>
        /// Creates a new <see cref="EntityDto{TPrimaryKey}"/> object.
        /// </summary>
        public EntityDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityDto{TPrimaryKey}"/> object.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        public EntityDto(TPrimaryKey id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"[{GetType().Name}] Id = {Id}";
        }
    }
}