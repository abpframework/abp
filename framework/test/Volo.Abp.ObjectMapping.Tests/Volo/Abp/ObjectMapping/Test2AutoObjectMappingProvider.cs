namespace Volo.Abp.ObjectMapping
{
    public class Test2AutoObjectMappingProvider<TContext> : IAutoObjectMappingProvider<TContext>
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
}