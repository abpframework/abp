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
    TSource ReverseMap(TDestination source);

    void ReverseMap(TDestination source, TSource destination);

    void BeforeReverseMap(TDestination source);

    void AfterReverseMap(TDestination source, TSource destination);
}
