using System;
using System.Diagnostics;
using System.Threading.Tasks;
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

        public async Task RunAsync()
        {
            await BeforeExecuteAsync();

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await ExecuteAsync();
                SuccessCount++;
            }
            catch
            {
                //TODO: Log!
                FailCount++;
            }
            finally
            {
                stopwatch.Stop();

                ExecutionCount++;

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
                DisplayText = GetDisplayText(),
                ExecutionCount = ExecutionCount,
                LastExecutionDuration = LastExecutionDuration,
                MaxExecutionDuration = MaxExecutionDuration,
                MinExecutionDuration = MinExecutionDuration,
                TotalExecutionDuration = TotalExecutionDuration,
                AvgExecutionDuration = ExecutionCount == 0 
                    ? 0.0 
                    : TotalExecutionDuration / ExecutionCount,
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