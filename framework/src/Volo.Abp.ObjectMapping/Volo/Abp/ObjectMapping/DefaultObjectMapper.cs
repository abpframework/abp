using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ObjectMapping;

public class DefaultObjectMapper<TContext> : DefaultObjectMapper, IObjectMapper<TContext>
{
    public DefaultObjectMapper(
        IServiceProvider serviceProvider,
        IAutoObjectMappingProvider<TContext> autoObjectMappingProvider
        ) : base(
            serviceProvider,
            autoObjectMappingProvider)
    {

    }
}

public class DefaultObjectMapper : IObjectMapper, ITransientDependency
{
    public IAutoObjectMappingProvider AutoObjectMappingProvider { get; }
    protected IServiceProvider ServiceProvider { get; }

    public DefaultObjectMapper(
        IServiceProvider serviceProvider,
        IAutoObjectMappingProvider autoObjectMappingProvider)
    {
        AutoObjectMappingProvider = autoObjectMappingProvider;
        ServiceProvider = serviceProvider;
    }

    //TODO: It can be slow to always check if service is available. Test it and optimize if necessary.

    public virtual TDestination Map<TSource, TDestination>(TSource source)
    {
        if (source == null)
        {
            return default;
        }

        using (var scope = ServiceProvider.CreateScope())
        {
            var specificMapper = scope.ServiceProvider.GetService<IObjectMapper<TSource, TDestination>>();
            if (specificMapper != null)
            {
                return specificMapper.Map(source);
            }
        }

        if (source is IMapTo<TDestination> mapperSource)
        {
            return mapperSource.MapTo();
        }

        if (typeof(IMapFrom<TSource>).IsAssignableFrom(typeof(TDestination)))
        {
            try
            {
                //TODO: Check if TDestination has a proper constructor which takes TSource
                //TODO: Check if TDestination has an empty constructor (in this case, use MapFrom)

                return (TDestination)Activator.CreateInstance(typeof(TDestination), source);
            }
            catch
            {
                //TODO: Remove catch when TODOs are implemented above
            }
        }

        return AutoMap<TSource, TDestination>(source);
    }

    public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (source == null)
        {
            return default;
        }

        using (var scope = ServiceProvider.CreateScope())
        {
            var specificMapper = scope.ServiceProvider.GetService<IObjectMapper<TSource, TDestination>>();
            if (specificMapper != null)
            {
                return specificMapper.Map(source, destination);
            }
        }

        if (source is IMapTo<TDestination> mapperSource)
        {
            mapperSource.MapTo(destination);
            return destination;
        }

        if (destination is IMapFrom<TSource> mapperDestination)
        {
            mapperDestination.MapFrom(source);
            return destination;
        }

        return AutoMap(source, destination);
    }

    protected virtual TDestination AutoMap<TSource, TDestination>(object source)
    {
        return AutoObjectMappingProvider.Map<TSource, TDestination>(source);
    }

    protected virtual TDestination AutoMap<TSource, TDestination>(TSource source, TDestination destination)
    {
        return AutoObjectMappingProvider.Map<TSource, TDestination>(source, destination);
    }
}
