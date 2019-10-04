using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Volo.ClientSimulation.Snapshot;

namespace Volo.ClientSimulation.Scenarios
{
    public abstract class ScenarioStep
    {
        protected int ExecutionCount;
        protected int SuccessCount;
        protected int FailCount;
        protected double TotalExecutionDuration;
        protected double MinExecutionDuration;
        protected double MaxExecutionDuration;
        protected double LastExecutionDuration;

        public async Task RunAsync(ScenarioExecutionContext context)
        {
            await BeforeExecuteAsync(context);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await ExecuteAsync(context);

                SuccessCount++;

                LastExecutionDuration = stopwatch.Elapsed.TotalMilliseconds;

                TotalExecutionDuration += LastExecutionDuration;

                if (MinExecutionDuration > LastExecutionDuration)
                {
                    MinExecutionDuration = LastExecutionDuration;
                }

                if (MaxExecutionDuration < LastExecutionDuration)
                {
                    MaxExecutionDuration = LastExecutionDuration;
                }
            }
            catch(Exception ex)
            {
                FailCount++;

                context
                    .ServiceProvider
                    .GetService<ILogger<ScenarioStep>>()
                    .LogException(ex);
            }
            finally
            {
                stopwatch.Stop();

                ExecutionCount++;
            }

            await AfterExecuteAsync(context);
        }

        protected virtual Task BeforeExecuteAsync(ScenarioExecutionContext context)
        {
            return Task.CompletedTask;
        }

        protected abstract Task ExecuteAsync(ScenarioExecutionContext context);

        protected virtual Task AfterExecuteAsync(ScenarioExecutionContext context)
        {
            return Task.CompletedTask;
        }

        public virtual string GetDisplayText()
        {
            var displayNameAttr = GetType()
                .GetCustomAttributes(true)
                .OfType<DisplayNameAttribute>()
                .FirstOrDefault();

            if (displayNameAttr != null)
            {
                return displayNameAttr.DisplayName;
            }

            return GetType()
                .Name
                .RemovePostFix(nameof(ScenarioStep));
        }

        public ScenarioStepSnapshot CreateSnapshot()
        {
            return new ScenarioStepSnapshot
            {
                DisplayText = GetDisplayText(),
                ExecutionCount = ExecutionCount,
                LastExecutionDuration = LastExecutionDuration,
                MaxExecutionDuration = MaxExecutionDuration,
                MinExecutionDuration = MinExecutionDuration,
                TotalExecutionDuration = TotalExecutionDuration,
                AvgExecutionDuration = SuccessCount == 0 
                    ? 0.0 
                    : TotalExecutionDuration / SuccessCount,
                FailCount = FailCount,
                SuccessCount = SuccessCount
            };
        }

        public virtual void Reset()
        {
            ExecutionCount = 0;
            FailCount = 0;
            SuccessCount = 0;
            TotalExecutionDuration = 0;
            MinExecutionDuration = 0.0;
            MaxExecutionDuration = 0.0;
            LastExecutionDuration = 0.0;
        }
    }
}