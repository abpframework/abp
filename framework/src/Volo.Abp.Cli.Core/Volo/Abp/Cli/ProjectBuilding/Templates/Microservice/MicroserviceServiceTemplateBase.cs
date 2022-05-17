using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;

public abstract class MicroserviceServiceTemplateBase : TemplateInfo
{
    protected MicroserviceServiceTemplateBase([NotNull] string name)
        : base(name)
    {
    }

    public static bool IsMicroserviceTemplate(string templateName)
    {
        return templateName == MicroserviceProTemplate.TemplateName;
    }

    public static bool IsMicroserviceServiceTemplate(string templateName)
    {
        return templateName == MicroserviceServiceProTemplate.TemplateName;
    }

    public static string CalculateTargetFolder(string mainSolutionFolder, string serviceName)
    {
        serviceName = serviceName.ToCamelCase().RemovePostFix("Service");

        return Path.Combine(mainSolutionFolder, "services", serviceName);
    }

    public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
    {
        var steps = new List<ProjectBuildPipelineStep>();

        DeleteUnrelatedUiProject(context, steps);
        SetRandomPortForHostProject(context, steps);
        RandomizeStringEncryption(context, steps);

        return steps;
    }

    private static void DeleteUnrelatedUiProject(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        switch (context.BuildArgs.UiFramework)
        {
            case UiFramework.Blazor:
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MicroserviceName.Web"));
                break;
            case UiFramework.Mvc:
            case UiFramework.NotSpecified:
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MicroserviceName.Blazor"));
                break;
            default:
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MicroserviceName.Blazor"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MicroserviceName.Web"));
                break;
        }
    }

    private static void SetRandomPortForHostProject(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new MicroserviceServiceRandomPortStep("44371"));
    }

    private static void RandomizeStringEncryption(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new MicroserviceServiceStringEncryptionStep());
    }
}
