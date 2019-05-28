using System;
using System.Linq.Expressions;

namespace Volo.Abp.Specifications
{
    /// <summary>
    /// Represents the specification which is represented by the corresponding
    /// LINQ expression.
    /// </summary>
    /// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
    public class ExpressionSpecification<T> : Specification<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        /// <summary>
        /// Initializes a new instance of <c>ExpressionSpecification&lt;T&gt;</c> class.
        /// </summary>
        /// <param name="expression">The LINQ expression which represents the current
        /// specification.</param>
        public ExpressionSpecification(Expression<Func<T, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Gets the LINQ expression which represents the current specification.
        /// </summary>
        /// <returns>The LINQ expression.</returns>
        public override Expression<Func<T, bool>> ToExpression()
        {
            return _expression;
        }
    }
}