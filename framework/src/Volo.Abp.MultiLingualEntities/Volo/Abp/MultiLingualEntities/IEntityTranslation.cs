namespace Volo.Abp.MultiLingualEntities
{
    public interface IEntityTranslation
    {
        string Language { get; set; }
    }

    public interface IEntityTranslation<TEntity, TPrimaryKeyOfMultiLingualEntity> : IEntityTranslation
    {
        TEntity Core { get; set; }

        TPrimaryKeyOfMultiLingualEntity CoreId { get; set; }
    }

    public interface IEntityTranslation<TEntity>: IEntityTranslation<TEntity, int>
    {

    }
}
