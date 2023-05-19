namespace Volo.Abp.Specifications;

/// <summary>
/// Represents the base class for composite specifications.
/// </summary>
/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
public abstract class CompositeSpecification<T> : Specification<T>, ICompositeSpecification<T>
{
    /// <summary>
    /// Constructs a new instance of <see cref="CompositeSpecification{T}"/> class.
    /// </summary>
    /// <param name="left">The first specification.</param>
    /// <param name="right">The second specification.</param>
    protected CompositeSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        Left = left;
        Right = right;
    }

    /// <summary>
    /// Gets the first specification.
    /// </summary>
    public ISpecification<T> Left { get; }

    /// <summary>
    /// Gets the second specification.
    /// </summary>
    public ISpecification<T> Right { get; }
}
