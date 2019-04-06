using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;

namespace Volo.ClientSimulation.Scenarios
{
    public abstract class Scenario : IScenario
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
                    $"No Steps defined for the scenario '{GetDisplayText()}'"
                );
            }
        }

        protected void AddStep(IScenarioStep step)
        {
            StepList.Add(step);
        }
    }

    public interface IScenario
    {
        IReadOnlyList<IScenarioStep> Steps { get; }

        IScenarioStep CurrentStep { get; }

        int CurrentStepIndex { get; }

        string GetDisplayText();

        void Proceed();
    }

    public interface IScenarioStep
    {
        void Run();

        string GetDisplayText();
    }

    public abstract class ScenarioStep : IScenarioStep
    {
        public abstract void Run();

        public virtual string GetDisplayText()
        {
            return GetType()
                .Name
                .RemovePostFix(nameof(ScenarioStep));
        }
    }

    public class DemoScenario : Scenario
    {
        public DemoScenario()
        {
            AddStep(new SleepScenarioStep());
            AddStep(new SleepScenarioStep(3000));
            AddStep(new SleepScenarioStep());
        }
    }

    public class SleepScenarioStep : ScenarioStep
    {
        public int Duration { get; }

        public SleepScenarioStep(int duration = 1000)
        {
            Duration = duration;
        }

        public override void Run()
        {
            Thread.Sleep(Duration);
        }
    }
}
