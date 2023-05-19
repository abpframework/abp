namespace Volo.Abp.ObjectMapping;

/// <summary>
/// Defines a simple interface to automatically map objects.
/// </summary>
public interface IObjectMapper
{
    /// <summary>
    /// Gets the underlying <see cref="IAutoObjectMappingProvider"/> object that is used for auto object mapping.
    /// </summary>
    IAutoObjectMappingProvider AutoObjectMappingProvider { get; }

    /// <summary>
    /// Converts an object to another. Creates a new object of <see cref="TDestination"/>.
    /// </summary>
    /// <typeparam name="TDestination">Type of the destination object</typeparam>
    /// <typeparam name="TSource">Type of the source object</typeparam>
    /// <param name="source">Source object</param>
    TDestination Map<TSource, TDestination>(TSource source);

    /// <summary>
    /// Execute a mapping from the source object to the existing destination object
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestination">Destination type</typeparam>
    /// <param name="source">Source object</param>
    /// <param name="destination">Destination object</param>
    /// <returns>Returns the same <see cref="destination"/> object after mapping operation</returns>
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}

/// <summary>
/// Defines a simple interface to automatically map objects for a specific context.
/// </summary>
public interface IObjectMapper<TContext> : IObjectMapper
{

}

/// <summary>
/// Maps an object to another.
/// Implement this interface to override object to object mapping for specific types.
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDestination"></typeparam>
public interface IObjectMapper<in TSource, TDestination>
{
    /// <summary>
    /// Converts an object to another. Creates a new object of <see cref="TDestination"/>.
    /// </summary>
    /// <param name="source">Source object</param>
    TDestination Map(TSource source);

    /// <summary>
    /// Execute a mapping from the source object to the existing destination object
    /// </summary>
    /// <param name="source">Source object</param>
    /// <param name="destination">Destination object</param>
    /// <returns>Returns the same <see cref="destination"/> object after mapping operation</returns>
    TDestination Map(TSource source, TDestination destination);
}
