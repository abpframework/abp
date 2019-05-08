namespace Volo.Abp.Specifications
{
    /// <summary>
    /// Represents that the implemented classes are specification parsers that
    /// parses the given specification to a domain specific criteria object, such 
    /// as the <c>ICriteria</c> instance in NHibernate.
    /// </summary>
    /// <typeparam name="TCriteria">The type of the domain specific criteria.</typeparam>
    public interface ISpecificationParser<out TCriteria>
    {
        /// <summary>
        /// Parses the given specification to a domain specific criteria object.
        /// </summary>
        /// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
        /// <param name="specification">The specified specification instance.</param>
        /// <returns>The instance of the domain specific criteria.</returns>
        TCriteria Parse<T>(ISpecification<T> specification);
    }
}