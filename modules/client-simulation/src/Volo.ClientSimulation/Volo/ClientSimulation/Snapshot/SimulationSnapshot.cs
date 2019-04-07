using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.ClientSimulation.Snapshot
{
    [Serializable]
    public class SimulationSnapshot
    {
        public SimulationState State { get; set; }

        public List<ClientSnapshot> Clients { get; set; }

        public List<ScenarioSummarySnapshot> Scenarios { get; set; }

        public void CreateSummaries()
        {
            var scenarioDictionary = new Dictionary<string, ScenarioSummarySnapshot>();

            foreach (var client in Clients)
            {
                var scenarioSummary = scenarioDictionary.GetOrAdd(
                    client.Scenario.DisplayText,
                    () => new ScenarioSummarySnapshot
                    {
                        DisplayText = client.Scenario.DisplayText,
                        Steps = new List<ScenarioStepSummarySnapshot>()
                    }
                );

                foreach (var scenarioStep in client.Scenario.Steps)
                {
                    var scenarioStepSummary = scenarioSummary.Steps.FirstOrDefault(s => s.DisplayText == scenarioStep.DisplayText);
                    if (scenarioStepSummary == null)
                    {
                        scenarioStepSummary = new ScenarioStepSummarySnapshot
                        {
                            DisplayText = scenarioStep.DisplayText
                        };

                        scenarioSummary.Steps.Add(scenarioStepSummary);
                    }

                    scenarioStepSummary.ExecutionCount += scenarioStep.ExecutionCount;
                    scenarioStepSummary.SuccessCount += scenarioStep.SuccessCount;
                    scenarioStepSummary.FailCount += scenarioStep.FailCount;
                    scenarioStepSummary.TotalExecutionDuration += scenarioStep.TotalExecutionDuration;

                    if (scenarioStepSummary.MinExecutionDuration > scenarioStep.MinExecutionDuration)
                    {
                        scenarioStepSummary.MinExecutionDuration = scenarioStep.MinExecutionDuration;
                    }

                    if (scenarioStepSummary.MaxExecutionDuration < scenarioStep.MaxExecutionDuration)
                    {
                        scenarioStepSummary.MaxExecutionDuration = scenarioStep.MaxExecutionDuration;
                    }

                    scenarioStepSummary.AvgExecutionDuration = scenarioStepSummary.SuccessCount == 0
                        ? 0.0
                        : scenarioStepSummary.TotalExecutionDuration / scenarioStepSummary.SuccessCount;
                }
            }

            Scenarios = scenarioDictionary.Values.ToList();
        }
    }
}
