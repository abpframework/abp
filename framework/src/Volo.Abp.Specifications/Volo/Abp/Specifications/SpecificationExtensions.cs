using JetBrains.Annotations;

namespace Volo.Abp.Specifications;

public static class SpecificationExtensions
{
    /// <summary>
    /// Combines the current specification instance with another specification instance
    /// and returns the combined specification which represents that both the current and
    /// the given specification must be satisfied by the given object.
    /// </summary>
    /// <param name="specification">The specification</param>
    /// <param name="other">The specification instance with which the current specification is combined.</param>
    /// <returns>The combined specification instance.</returns>
    public static ISpecification<T> And<T>([NotNull] this ISpecification<T> specification,
        [NotNull] ISpecification<T> other)
    {
        Check.NotNull(specification, nameof(specification));
        Check.NotNull(other, nameof(other));

        return new AndSpecification<T>(specification, other);
    }

    /// <summary>
    /// Combines the current specification instance with another specification instance
    /// and returns the combined specification which represents that either the current or
    /// the given specification should be satisfied by the given object.
    /// </summary>
    /// <param name="specification">The specification</param>
    /// <param name="other">The specification instance with which the current specification
    /// is combined.</param>
    /// <returns>The combined specification instance.</returns>
    public static ISpecification<T> Or<T>([NotNull] this ISpecification<T> specification,
        [NotNull] ISpecification<T> other)
    {
        Check.NotNull(specification, nameof(specification));
        Check.NotNull(other, nameof(other));

        return new OrSpecification<T>(specification, other);
    }

    /// <summary>
    /// Combines the current specification instance with another specification instance
    /// and returns the combined specification which represents that the current specification
    /// should be satisfied by the given object but the specified specification should not.
    /// </summary>
    /// <param name="specification">The specification</param>
    /// <param name="other">The specification instance with which the current specification
    /// is combined.</param>
    /// <returns>The combined specification instance.</returns>
    public static ISpecification<T> AndNot<T>([NotNull] this ISpecification<T> specification,
        [NotNull] ISpecification<T> other)
    {
        Check.NotNull(specification, nameof(specification));
        Check.NotNull(other, nameof(other));

        return new AndNotSpecification<T>(specification, other);
    }

    /// <summary>
    /// Reverses the current specification instance and returns a specification which represents
    /// the semantics opposite to the current specification.
    /// </summary>
    /// <returns>The reversed specification instance.</returns>
    public static ISpecification<T> Not<T>([NotNull] this ISpecification<T> specification)
    {
        Check.NotNull(specification, nameof(specification));

        return new NotSpecification<T>(specification);
    }
}
