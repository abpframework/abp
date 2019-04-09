using System;
using Volo.ClientSimulation.Clients;

namespace Volo.ClientSimulation.Snapshot
{
    [Serializable]
    public class ClientSnapshot
    {
        public ClientState State { get; set; }

        public ScenarioSnapshot Scenario { get; set; }
    }
}