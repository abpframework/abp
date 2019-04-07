using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.ClientSimulation.Scenarios
{
    public abstract class Scenario : ITransientDependency
    {
        public IReadOnlyList<ScenarioStep> Steps => StepList.ToImmutableList();
        protected List<ScenarioStep> StepList { get; }

        public ScenarioStep CurrentStep
        {
            get
            {
                CheckStepCount();
                return StepList[CurrentStepIndex];
            }
        }
        public int CurrentStepIndex { get; protected set; }

        protected Scenario()
        {
            StepList = new List<ScenarioStep>();
        }

        public virtual string GetDisplayText()
        {
            return GetType()
                .Name
                .RemovePostFix(nameof(Scenario));
        }

        public virtual async Task ProceedAsync()
        {
            CheckStepCount();

            await StepList[CurrentStepIndex].RunAsync();

            CurrentStepIndex++;

            if (CurrentStepIndex >= StepList.Count)
            {
                CurrentStepIndex = 0;
            }
        }

        public void Reset()
        {
            CurrentStepIndex = 0;
        }

        private void CheckStepCount()
        {
            if (StepList.Count <= 0)
            {
                throw new ApplicationException(
                    $"No Steps added to the scenario '{GetDisplayText()}'"
                );
            }
        }

        protected void AddStep(ScenarioStep step)
        {
            StepList.Add(step);
        }
    }
}
