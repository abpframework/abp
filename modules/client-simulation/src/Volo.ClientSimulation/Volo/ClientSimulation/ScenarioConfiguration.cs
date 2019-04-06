using System;

namespace Volo.ClientSimulation
{
    public class ScenarioConfiguration
    {
        public Type ScenarioType { get; }

        public int ClientCount { get; }

        public ScenarioConfiguration(
            Type scenarioType,
            int clientCount = 1)
        {
            ScenarioType = scenarioType;
            ClientCount = clientCount;
        }
    }
}