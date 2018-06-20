using AutoMapper;

namespace Volo.Abp.AutoMapper
{
    public static class AutoMapExtensions
    {
        /// <summary>
        /// Converts an object to another using AutoMapper library. Creates a new object of <typeparamref name="TDestination"/>.
        /// There must be a mapping between objects before calling this method.
        /// </summary>
        /// <typeparam name="TDestination">Type of the destination object</typeparam>
        /// <param name="source">Source object</param>
        public static TDestination MapTo<TDestination>(this object source)
        {
            return Mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// Execute a mapping from the source object to the existing destination object
        /// There must be a mapping between objects before calling this method.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <returns></returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }
    }
}
