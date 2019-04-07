using System;
using System.Collections.Generic;

namespace Volo.ClientSimulation.Snapshot
{
    [Serializable]
    public class SimulationSnapshot
    {
        public SimulationState State { get; set; }

        public List<ClientSnapshot> Clients { get; set; }
    }
}
