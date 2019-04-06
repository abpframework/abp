namespace Volo.ClientSimulation.Scenarios
{
    public interface IScenarioStep
    {
        void Run();

        string GetDisplayText();
    }
}