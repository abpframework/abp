using System;

namespace Volo.ClientSimulation.Snapshot
{
    [Serializable]
    public class ScenarioStepSummarySnapshot
    {
        public string DisplayText { get; set; }

        public int ExecutionCount { get; set; }

        public int SuccessCount { get; set; }

        public int FailCount { get; set; }

        public double AvgExecutionDuration { get; set; }

        public double TotalExecutionDuration { get; set; }

        public double MinExecutionDuration { get; set; }

        public double MaxExecutionDuration { get; set; }
    }
}