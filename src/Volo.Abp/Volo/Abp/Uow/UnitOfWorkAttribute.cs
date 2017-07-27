using System;

namespace Volo.Abp.Uow
{
    /// <summary>
    /// Used to indicate that declaring method (or all methods of the class) is atomic and should be considered as a unit of work (UOW).
    /// </summary>
    /// <remarks>
    /// This attribute has no effect if there is already a unit of work before calling this method. It uses the ambient UOW in this case.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class UnitOfWorkAttribute : Attribute
    {
    }
}
