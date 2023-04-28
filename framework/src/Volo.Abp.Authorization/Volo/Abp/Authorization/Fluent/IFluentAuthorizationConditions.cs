using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization.Fluent;

public interface IFluentAuthorizationConditions
{
    void IsGranted(object resource, IAuthorizationRequirement requirement);

    void IsGranted(object resource, AuthorizationPolicy policy);

    void IsGranted(AuthorizationPolicy policy);

    void IsGranted(object resource, IEnumerable<IAuthorizationRequirement> requirements);

    void IsGranted(object resource, string policyName);

    void Condition(Func<bool> condition, [CanBeNull] Exception exceptionForFailure);

    void Condition(Func<Task<bool>> condition, [CanBeNull] Exception exceptionForFailure);
}