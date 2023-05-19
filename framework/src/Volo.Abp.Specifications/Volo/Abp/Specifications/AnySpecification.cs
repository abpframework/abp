using System;
using System.Linq.Expressions;

namespace Volo.Abp.Specifications;

/// <summary>
/// Represents the specification that can be satisfied by the given object
/// in any circumstance.
/// </summary>
/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
public sealed class AnySpecification<T> : Specification<T>
{
    /// <summary>
    /// Gets the LINQ expression which represents the current specification.
    /// </summary>
    /// <returns>The LINQ expression.</returns>
    public override Expression<Func<T, bool>> ToExpression()
    {
        return o => true;
    }
}
