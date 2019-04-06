using System;

namespace Volo.ClientSimulation.Scenarios
{
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
}