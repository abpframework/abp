using System;
using System.Linq.Expressions;

namespace Volo.Abp.Specifications;

/// <summary>
/// Represents the specification which indicates the semantics opposite to the given specification.
/// </summary>
/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
public class NotSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _specification;

    /// <summary>
    /// Initializes a new instance of <see cref="NotSpecification{T}"/> class.
    /// </summary>
    /// <param name="specification">The specification to be reversed.</param>
    public NotSpecification(ISpecification<T> specification)
    {
        _specification = specification;
    }

    /// <summary>
    /// Gets the LINQ expression which represents the current specification.
    /// </summary>
    /// <returns>The LINQ expression.</returns>
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = _specification.ToExpression();
        return Expression.Lambda<Func<T, bool>>(
            Expression.Not(expression.Body),
            expression.Parameters
        );
    }
}
