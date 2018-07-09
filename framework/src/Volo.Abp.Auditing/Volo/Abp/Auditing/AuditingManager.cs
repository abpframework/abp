using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Auditing
{
    public class AuditingManager : IAuditingManager, ITransientDependency
    {
        private const string AmbientContextKey = "Volo.Abp.Auditing.IAuditLogScope";

        private readonly IAmbientScopeProvider<IAuditLogScope> _ambientScopeProvider;
        private readonly IAuditingHelper _auditingHelper;
        private readonly IAuditingStore _auditingStore;

        public AuditingManager(
            IAmbientScopeProvider<IAuditLogScope> ambientScopeProvider, 
            IAuditingHelper auditingHelper, 
            IAuditingStore auditingStore)
        {
            _ambientScopeProvider = ambientScopeProvider;
            _auditingHelper = auditingHelper;
            _auditingStore = auditingStore;
        }

        public IAuditLogScope Current => _ambientScopeProvider.GetValue(AmbientContextKey);

        public IAuditLogSaveHandle BeginScope()
        {
            var ambientScope = _ambientScopeProvider.BeginScope(
                AmbientContextKey,
                new AuditLogScope(_auditingHelper.CreateAuditLogInfo())
            );

            Debug.Assert(Current != null, "Current != null");

            var stopWatch = Stopwatch.StartNew();

            return new DisposableSaveHandle(_auditingStore, ambientScope, Current.Log, stopWatch);
        }

        private class DisposableSaveHandle : IAuditLogSaveHandle
        {
            private readonly IAuditingStore _auditingStore;
            private readonly IDisposable _scope;
            private readonly AuditLogInfo _auditLog;
            private readonly Stopwatch _stopWatch;

            private bool _saved;

            public DisposableSaveHandle(IAuditingStore auditingStore, IDisposable scope, AuditLogInfo auditLog, Stopwatch stopWatch)
            {
                _auditingStore = auditingStore;
                _scope = scope;
                _auditLog = auditLog;
                _stopWatch = stopWatch;
            }

            private void BeforeSave()
            {
                _stopWatch.Stop();
                _saved = true;
                _auditLog.ExecutionDuration = Convert.ToInt32(_stopWatch.Elapsed.TotalMilliseconds);
            }

            public async Task SaveAsync()
            {
                BeforeSave();
                await _auditingStore.SaveAsync(_auditLog);
            }

            public void Save()
            {
                BeforeSave();
                _auditingStore.Save(_auditLog);
            }

            public void Dispose()
            {
                if (!_saved)
                {
                    Save();
                }

                _scope.Dispose();
            }
        }
    }
}