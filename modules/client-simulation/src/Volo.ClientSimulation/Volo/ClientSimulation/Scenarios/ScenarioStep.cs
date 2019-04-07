using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Volo.ClientSimulation.Snapshot;

namespace Volo.ClientSimulation.Scenarios
{
    public abstract class ScenarioStep
    {
        public async Task RunAsync()
        {
            await BeforeExecuteAsync();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await ExecuteAsync();
            }
            catch
            {

            }
            finally
            {
                stopwatch.Stop();
            }

            await AfterExecuteAsync();
        }

        protected virtual Task BeforeExecuteAsync()
        {
            return Task.CompletedTask;
        }

        protected abstract Task ExecuteAsync();

        protected virtual Task AfterExecuteAsync()
        {
            return Task.CompletedTask;
        }

        public virtual string GetDisplayText()
        {
            return GetType()
                .Name
                .RemovePostFix(nameof(ScenarioStep));
        }

        public ScenarioStepSnapshot CreateSnapshot()
        {
            return new ScenarioStepSnapshot
            {
                DisplayText = GetDisplayText()
            };
        }
    }
}