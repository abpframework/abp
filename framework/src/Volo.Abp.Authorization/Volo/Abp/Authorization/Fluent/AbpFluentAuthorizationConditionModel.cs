using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Fluent;

public class AbpFluentAuthorizationConditionModel : IAbpFluentAuthorizationModel
{
    public Func<Task<bool>> ConditionFunc { get; set; }

    /// <summary>
    /// It throws this exception if this node is not authorized. 
    /// And if this exception is null, the default exception is thrown.
    /// </summary>
    [CanBeNull]
    public Exception ExceptionForFailure { get; }

    public AbpFluentAuthorizationConditionModel(Func<bool> conditionFunc, [CanBeNull] Exception exceptionForFailure)
    {
        ConditionFunc = () => Task.FromResult(conditionFunc());
        ExceptionForFailure = exceptionForFailure;
    }

    public AbpFluentAuthorizationConditionModel(Func<Task<bool>> conditionFunc,
        [CanBeNull] Exception exceptionForFailure)
    {
        ConditionFunc = conditionFunc;
        ExceptionForFailure = exceptionForFailure;
    }
}