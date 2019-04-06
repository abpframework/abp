using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Volo.Abp.DependencyInjection;

namespace Volo.ClientSimulation.Scenarios
{
    public abstract class Scenario : IScenario, ITransientDependency
    {
        public IReadOnlyList<IScenarioStep> Steps => StepList.ToImmutableList();
        protected List<IScenarioStep> StepList { get; }

        public IScenarioStep CurrentStep
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
            StepList = new List<IScenarioStep>();
        }

        public virtual string GetDisplayText()
        {
            return GetType()
                .Name
                .RemovePostFix(nameof(Scenario));
        }

        public virtual void Proceed()
        {
            CheckStepCount();

            StepList[CurrentStepIndex].Run();

            CurrentStepIndex++;

            if (CurrentStepIndex >= StepList.Count)
            {
                CurrentStepIndex = 0;
            }
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

        protected void AddStep(IScenarioStep step)
        {
            StepList.Add(step);
        }
    }
}
