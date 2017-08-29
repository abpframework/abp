using System;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// A shortcut of <see cref="IEntityDto{TPrimaryKey}"/> for default primary key type (<see cref="Guid"/>).
    /// </summary>
    public interface IEntityDto : IEntityDto<Guid>
    {

    }

    /// <summary>
    /// Defines common properties for entity based DTOs.
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Id of the entity.
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}