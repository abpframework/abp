using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Auditing
{
    public class AuditingManager : IAuditingManager, ITransientDependency
    {
        private const string AmbientContextKey = "Volo.Abp.Auditing.IAuditLogScope";

        protected IServiceProvider ServiceProvider { get; }
        protected AbpAuditingOptions Options { get; }
        protected ILogger<AuditingManager> Logger { get; set; }
        private readonly IAmbientScopeProvider<IAuditLogScope> _ambientScopeProvider;
        private readonly IAuditingHelper _auditingHelper;
        private readonly IAuditingStore _auditingStore;

        public AuditingManager(
            IAmbientScopeProvider<IAuditLogScope> ambientScopeProvider, 
            IAuditingHelper auditingHelper, 
            IAuditingStore auditingStore,
            IServiceProvider serviceProvider,
            IOptions<AbpAuditingOptions> options)
        {
            ServiceProvider = serviceProvider;
            Options = options.Value;
            Logger = NullLogger<AuditingManager>.Instance;

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

            return new DisposableSaveHandle(this, ambientScope, Current.Log, Stopwatch.StartNew());
        }

        protected virtual void ExecutePostContributors(AuditLogInfo auditLogInfo)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new AuditLogContributionContext(scope.ServiceProvider, auditLogInfo);

                foreach (var contributor in Options.Contributors)
                {
                    try
                    {
                        contributor.PostContribute(context);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex, LogLevel.Warning);
                    }
                }
            }
        }

        protected virtual void BeforeSave(DisposableSaveHandle saveHandle)
        {
            saveHandle.StopWatch.Stop();
            saveHandle.AuditLog.ExecutionDuration = Convert.ToInt32(saveHandle.StopWatch.Elapsed.TotalMilliseconds);
            ExecutePostContributors(saveHandle.AuditLog);
        }

        protected virtual async Task SaveAsync(DisposableSaveHandle saveHandle)
        {
            BeforeSave(saveHandle);
            await _auditingStore.SaveAsync(saveHandle.AuditLog);
        }

        protected virtual void Save(DisposableSaveHandle saveHandle)
        {
            BeforeSave(saveHandle);
            _auditingStore.Save(saveHandle.AuditLog);
        }

        protected class DisposableSaveHandle : IAuditLogSaveHandle
        {
            public AuditLogInfo AuditLog { get; }
            public Stopwatch StopWatch { get; }

            private readonly AuditingManager _auditingManager;
            private readonly IDisposable _scope;
            private bool _saved;

            public DisposableSaveHandle(
                AuditingManager auditingManager,
                IDisposable scope,
                AuditLogInfo auditLog, 
                Stopwatch stopWatch)
            {
                _auditingManager = auditingManager;
                _scope = scope;
                AuditLog = auditLog;
                StopWatch = stopWatch;
            }

            public async Task SaveAsync()
            {
                _saved = true;
                await _auditingManager.SaveAsync(this);
            }

            public void Save()
            {
                _saved = true;
                _auditingManager.Save(this);
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