using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.ClientSimulation.Snapshot;

namespace Volo.ClientSimulation.Scenarios;

public abstract class Scenario : ITransientDependency
{
    protected List<ScenarioStep> Steps { get; }

    protected ScenarioStep CurrentStep {
        get {
            CheckStepCount();
            return Steps[CurrentStepIndex];
        }
    }

    protected int CurrentStepIndex { get; set; }

    protected ScenarioExecutionContext ExecutionContext { get; }

    protected Scenario(IServiceProvider serviceProvider)
    {
        ExecutionContext = new ScenarioExecutionContext(serviceProvider);
        Steps = new List<ScenarioStep>();
    }

    public virtual string GetDisplayText()
    {
        var displayNameAttr = GetType()
            .GetCustomAttributes(true)
            .OfType<DisplayNameAttribute>()
            .FirstOrDefault();

        if (displayNameAttr != null)
        {
            return displayNameAttr.DisplayName;
        }

        return GetType()
            .Name
            .RemovePostFix(nameof(Scenario));
    }

    public virtual async Task ProceedAsync()
    {
        CheckStepCount();

        await Steps[CurrentStepIndex].RunAsync(ExecutionContext);

        CurrentStepIndex++;

        if (CurrentStepIndex >= Steps.Count)
        {
            CurrentStepIndex = 0;
        }
    }

    public void Reset()
    {
        CurrentStepIndex = 0;

        foreach (var step in Steps)
        {
            step.Reset();
        }

        ExecutionContext.Reset();
    }

    public ScenarioSnapshot CreateSnapshot()
    {
        return new ScenarioSnapshot
        {
            DisplayText = GetDisplayText(),
            Steps = Steps.Select(s => s.CreateSnapshot()).ToList(),
            CurrentStep = CurrentStep.CreateSnapshot()
        };
    }

    protected void AddStep(ScenarioStep step)
    {
        Steps.Add(step);
    }

    private void CheckStepCount()
    {
        if (Steps.Count <= 0)
        {
            throw new ApplicationException(
                $"No Steps added to the scenario '{GetDisplayText()}'"
            );
        }
    }
}
