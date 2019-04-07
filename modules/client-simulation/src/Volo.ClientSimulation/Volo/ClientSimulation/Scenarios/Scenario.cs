using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.ClientSimulation.Snapshot;

namespace Volo.ClientSimulation.Scenarios
{
    public abstract class Scenario : ITransientDependency
    {
        protected List<ScenarioStep> Steps { get; }

        protected ScenarioStep CurrentStep
        {
            get
            {
                CheckStepCount();
                return Steps[CurrentStepIndex];
            }
        }

        protected int CurrentStepIndex { get; set; }

        protected Scenario()
        {
            Steps = new List<ScenarioStep>();
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

            await Steps[CurrentStepIndex].RunAsync();

            CurrentStepIndex++;

            if (CurrentStepIndex >= Steps.Count)
            {
                CurrentStepIndex = 0;
            }
        }

        public void Reset()
        {
            CurrentStepIndex = 0;
        }

        public ScenarioSnapshot CreateSnapshot()
        {
            return new ScenarioSnapshot
            {
                DisplayText = GetDisplayText(),
                Steps = Steps.Select(s => s.CreateSnapshot()).ToList(),
                CurrentStep = CurrentStep.CreateSnapshot()
            };
        }

        protected void AddStep(ScenarioStep step)
        {
            Steps.Add(step);
        }

        private void CheckStepCount()
        {
            if (Steps.Count <= 0)
            {
                throw new ApplicationException(
                    $"No Steps added to the scenario '{GetDisplayText()}'"
                );
            }
        }
    }
}
