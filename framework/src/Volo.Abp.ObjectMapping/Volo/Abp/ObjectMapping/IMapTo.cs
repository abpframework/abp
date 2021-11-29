namespace Volo.Abp.ObjectMapping
{
    public interface IMapTo<TDestination>
    {
        TDestination MapTo();

        void MapTo(TDestination destination);
    }
}
