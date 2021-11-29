using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ObjectMapping;

public class Test1AutoObjectMappingProvider<TContext> : Test1AutoObjectMappingProvider, IAutoObjectMappingProvider<TContext>
{

}

public class Test1AutoObjectMappingProvider : IAutoObjectMappingProvider, ITransientDependency
{
    public TDestination Map<TSource, TDestination>(object source)
    {
        return default;
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return default;
    }
}
