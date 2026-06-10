namespace Volo.Abp.Application.Dtos;

public interface IEntityDto
{

}

public interface IEntityDto<TKey> : IEntityDto, IKeyedObject
{
    TKey Id { get; set; }
}
