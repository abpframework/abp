using System;
using System.Collections.Generic;
using System.Reflection;

namespace Volo.Abp.Auditing;

//TODO: Move ShouldSaveAudit & IsEntityHistoryEnabled and rename to IAuditingFactory
public interface IAuditingHelper
{
    bool ShouldSaveAudit(MethodInfo? methodInfo, bool defaultValue = false, bool ignoreIntegrationServiceAttribute = false);

    bool IsEntityHistoryEnabled(Type entityType, bool defaultValue = false);

    AuditLogInfo CreateAuditLogInfo();

    AuditLogActionInfo CreateAuditLogAction(
        AuditLogInfo auditLog,
        Type? type,
        MethodInfo method,
        object?[] arguments
    );

    AuditLogActionInfo CreateAuditLogAction(
        AuditLogInfo auditLog,
        Type? type,
        MethodInfo method,
        IDictionary<string, object?> arguments
    );

    /// <summary>
    /// Creates a scope in which auditing is temporarily disabled.
    /// </summary>
    /// <returns>
    /// An <see cref="IDisposable"/> that restores the previous auditing state
    /// when disposed. This method supports nested scopes; disposing a scope
    /// restores the auditing state that was active before that scope was created.
    /// </returns>
    IDisposable DisableAuditing();

    /// <summary>
    /// Determines whether auditing is currently enabled.
    /// </summary>
    /// <returns>
    /// <c>true</c> if auditing is enabled in the current context; otherwise, <c>false</c>.
    /// This reflects any active scopes created by <see cref="DisableAuditing"/>.
    /// </returns>
    bool IsAuditingEnabled();
}
