using System;
using System.Data;

namespace Volo.Abp.Uow;

/// <summary>
/// Used to indicate that declaring method (or all methods of the class) is atomic and should be considered as a unit of work (UOW).
/// </summary>
/// <remarks>
/// This attribute has no effect if there is already a unit of work before calling this method. It uses the ambient UOW in this case.
/// </remarks>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class UnitOfWorkAttribute : Attribute
{
    /// <summary>
    /// Is this UOW transactional?
    /// Uses default value if not supplied.
    /// </summary>
    public bool? IsTransactional { get; set; }

    /// <summary>
    /// Timeout of UOW As milliseconds.
    /// Uses default value if not supplied.
    /// </summary>
    public int? Timeout { get; set; }

    /// <summary>
    /// If this UOW is transactional, this option indicated the isolation level of the transaction.
    /// Uses default value if not supplied.
    /// </summary>
    public IsolationLevel? IsolationLevel { get; set; }

    /// <summary>
    /// Used to prevent starting a unit of work for the method.
    /// If there is already a started unit of work, this property is ignored.
    /// Default: false.
    /// </summary>
    public bool IsDisabled { get; set; }

    public UnitOfWorkAttribute()
    {

    }

    public UnitOfWorkAttribute(bool isTransactional)
    {
        IsTransactional = isTransactional;
    }

    public UnitOfWorkAttribute(bool isTransactional, IsolationLevel isolationLevel)
    {
        IsTransactional = isTransactional;
        IsolationLevel = isolationLevel;
    }

    public UnitOfWorkAttribute(bool isTransactional, IsolationLevel isolationLevel, int timeout)
    {
        IsTransactional = isTransactional;
        IsolationLevel = isolationLevel;
        Timeout = timeout;
    }

    //TODO: More constructors!

    public virtual void SetOptions(AbpUnitOfWorkOptions options)
    {
        if (IsTransactional.HasValue)
        {
            options.IsTransactional = IsTransactional.Value;
        }

        if (Timeout.HasValue)
        {
            options.Timeout = Timeout;
        }

        if (IsolationLevel.HasValue)
        {
            options.IsolationLevel = IsolationLevel;
        }
    }
}
