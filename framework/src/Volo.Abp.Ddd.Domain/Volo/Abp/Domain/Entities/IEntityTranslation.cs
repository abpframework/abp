namespace Volo.Abp.Domain.Entities
{
    public interface IEntityTranslation
    {
        string Language { get; set; }
    }

    public interface IEntityTranslation<TEntity, TPrimaryKeyOfMultiLingualEntity> : IEntityTranslation
        where TEntity : IEntity<TPrimaryKeyOfMultiLingualEntity>
    {
        TEntity Core { get; set; }

        TPrimaryKeyOfMultiLingualEntity CoreId { get; set; }
    }

    public interface IEntityTranslation<TEntity>: IEntityTranslation<TEntity, int>
        where TEntity : IEntity<int>
    {

    }
}
