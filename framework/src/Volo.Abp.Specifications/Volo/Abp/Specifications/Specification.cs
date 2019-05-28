using System;
using System.Linq.Expressions;

namespace Volo.Abp.Specifications
{
    /// <summary>
    /// Represents the base class for specifications.
    /// </summary>
    /// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
    public abstract class Specification<T> : ISpecification<T>
    {
        /// <summary>
        /// Returns a <see cref="bool"/> value which indicates whether the specification
        /// is satisfied by the given object.
        /// </summary>
        /// <param name="obj">The object to which the specification is applied.</param>
        /// <returns>True if the specification is satisfied, otherwise false.</returns>
        public virtual bool IsSatisfiedBy(T obj)
        {
            return ToExpression().Compile()(obj);
        }

        /// <summary>
        /// Gets the LINQ expression which represents the current specification.
        /// </summary>
        /// <returns>The LINQ expression.</returns>
        public abstract Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// Implicitly converts a specification to expression.
        /// </summary>
        /// <param name="specification"></param>
        public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
        {
            return specification.ToExpression();
        }
    }
}