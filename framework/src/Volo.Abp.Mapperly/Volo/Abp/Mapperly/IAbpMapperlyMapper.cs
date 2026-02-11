namespace Volo.Abp.Mapperly;

public interface IAbpMapperlyMapper<in TSource, TDestination>
{
    TDestination Map(TSource source);

    void Map(TSource source, TDestination destination);

    void BeforeMap(TSource source);

    void AfterMap(TSource source, TDestination destination);
}

public interface IAbpReverseMapperlyMapper<TSource, TDestination> : IAbpMapperlyMapper<TSource, TDestination>
{
    TSource ReverseMap(TDestination destination);

    void ReverseMap(TDestination destination, TSource source);

    void BeforeReverseMap(TDestination destination);

    void AfterReverseMap(TDestination destination, TSource source);
}
