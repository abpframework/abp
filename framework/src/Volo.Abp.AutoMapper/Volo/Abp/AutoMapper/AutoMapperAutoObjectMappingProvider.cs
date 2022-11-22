using System;
using AutoMapper;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.AutoMapper;

public class AutoMapperAutoObjectMappingProvider<TContext> : AutoMapperAutoObjectMappingProvider, IAutoObjectMappingProvider<TContext>
{
    public AutoMapperAutoObjectMappingProvider(IMapper mapper)
        : base(mapper)
    {
    }
}

public class AutoMapperAutoObjectMappingProvider : IAutoObjectMappingProvider, IDisposable
{
    public IMapper Mapper { get; }

    public AutoMapperAutoObjectMappingProvider(IMapper mapper)
    {
        Mapper = mapper;
    }

    public virtual TDestination Map<TSource, TDestination>(object source)
    {
        return Mapper.Map<TDestination>(source);
    }

    public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return Mapper.Map(source, destination);
    }

    public void Dispose()
    {
    }
}
