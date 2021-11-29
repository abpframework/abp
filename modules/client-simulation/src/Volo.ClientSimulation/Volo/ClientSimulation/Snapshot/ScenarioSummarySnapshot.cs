using System;
using System.Collections.Generic;

namespace Volo.ClientSimulation.Snapshot
{
    [Serializable]
    public class ScenarioSummarySnapshot
    {
        public string DisplayText { get; set; }

        public List<ScenarioStepSummarySnapshot> Steps { get; set; }
    }
}