using System.Collections.Generic;

namespace Volo.ClientSimulation.Scenarios
{
    public interface IScenario
    {
        IReadOnlyList<IScenarioStep> Steps { get; }

        IScenarioStep CurrentStep { get; }

        int CurrentStepIndex { get; }

        string GetDisplayText();

        void Proceed();
    }
}