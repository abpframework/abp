using System.Collections.Generic;
using Volo.ClientSimulation.Scenarios;

namespace Volo.ClientSimulation
{
    public class ClientSimulationOptions
    {
        public List<IScenario> Scenarios { get; }

        public ClientSimulationOptions()
        {
            Scenarios = new List<IScenario>();
        }
    }
}
