namespace Volo.Abp.Application.Dtos
{
    public interface IEntityDto
    {

    }

    public interface IEntityDto<TPrimaryKey> : IEntityDto
    {
        TPrimaryKey Id { get; set; }
    }
}