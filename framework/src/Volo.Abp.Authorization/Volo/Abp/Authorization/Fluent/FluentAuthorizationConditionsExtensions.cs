using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Volo.Abp.Authorization.Fluent;

public static class FluentAuthorizationConditionsExtensions
{
    public static void HasClaim(this IFluentAuthorizationConditions auth, string name,
        [CanBeNull] IEnumerable<string> values)
    {
        auth.IsGranted(null, new ClaimsAuthorizationRequirement(name, values));
    }

    public static void HasClaim(this IFluentAuthorizationConditions auth, string name, [NotNull] string value)
    {
        auth.IsGranted(null, new ClaimsAuthorizationRequirement(name, new[] { value }));
    }

    public static void HasClaim(this IFluentAuthorizationConditions auth, string name)
    {
        auth.IsGranted(null, new ClaimsAuthorizationRequirement(name, null));
    }

    public static void IsInRole(this IFluentAuthorizationConditions auth, string roleName)
    {
        auth.IsGranted(null, new RoleRequirement(roleName));
    }

    public static void IsInAllRoles(this IFluentAuthorizationConditions auth, string[] roleNames)
    {
        auth.IsGranted(null, new RolesRequirement(roleNames, true));
    }

    public static void IsInAnyRole(this IFluentAuthorizationConditions auth, string[] roleNames)
    {
        auth.IsGranted(null, new RolesRequirement(roleNames, false));
    }

    public static void IsGranted(this IFluentAuthorizationConditions auth, string policyName)
    {
        auth.IsGranted(null, new PermissionRequirement(policyName));
    }

    public static void IsGranted(this IFluentAuthorizationConditions auth, string[] policyNames)
    {
        auth.IsGranted(null, new PermissionsRequirement(policyNames, true));
    }

    public static void IsGrantedAny(this IFluentAuthorizationConditions auth, string[] policyNames)
    {
        auth.IsGranted(null, new PermissionsRequirement(policyNames, false));
    }

    public static void IsUser(this IFluentAuthorizationConditions auth, Guid userId)
    {
        auth.IsGranted(null, new UserRequirement(userId));
    }

    public static void IsUser(this IFluentAuthorizationConditions auth, string userName)
    {
        auth.IsGranted(null, new UserRequirement(userName));
    }

    public static void IsInUsers(this IFluentAuthorizationConditions auth, IEnumerable<Guid> userIds)
    {
        auth.IsGranted(null, new UsersRequirement(userIds));
    }

    public static void IsInUsers(this IFluentAuthorizationConditions auth, IEnumerable<string> userNames)
    {
        auth.IsGranted(null, new UsersRequirement(userNames));
    }
}