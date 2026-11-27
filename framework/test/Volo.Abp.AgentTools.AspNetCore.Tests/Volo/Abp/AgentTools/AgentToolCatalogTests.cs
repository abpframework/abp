using System;
using System.Linq;
using Shouldly;
using Volo.Abp.AgentTools.Sample;
using Volo.Abp.AgentTools.Sample.Binding;
using Xunit;

namespace Volo.Abp.AgentTools;

public class AgentToolCatalogTests : AgentToolsAspNetCoreTestBase
{
    private static readonly string[] AgentToolsSampleTools =
    [
        "Book_Archive",
        "Book_Create",
        "Book_CreateThenFail",
        "Book_Delete",
        "Book_Find",
        "Book_Get",
        "Book_GetCount",
        "Book_GetList",
        "Book_Update",
        "SampleReport_Create",
        "SampleReport_Delete",
        "SampleReport_Get",
        "SampleReport_Get_2"
    ];

    private static readonly string[] OtherSampleTools = ["OtherSample_GetGreeting", "OtherSample_Register"];

    // (controller type, action) => reason prefix, for the actions that must never be tools.
    private static readonly (Type Controller, string Action, string Reason)[] NeverExposed =
    [
        (typeof(BookAppService), nameof(IBookAppService.UploadCoverAsync), "non-JSON input"),
        (typeof(BookAppService), nameof(IBookAppService.GetCoverAsync), "non-JSON return"),
        (typeof(BookAppService), nameof(IBookAppService.GetPublicCountAsync), "[AllowAnonymous]"),
        (typeof(BookAppService), nameof(IBookAppService.ResetAllAsync), "[DisableAgentTool]"),
        (typeof(BookIntegrationService), nameof(IBookIntegrationService.GetAllAsync), "integration service"),
        (typeof(SampleReportController), nameof(ISampleReportAppService.ImportAsync), "non-JSON input"),
        (typeof(SampleReportController), nameof(ISampleReportAppService.GetLatestAsync), "[DisableAgentTool]"),
        (typeof(SampleReportController), nameof(ISampleReportAppService.GetPublicAsync), "[AllowAnonymous]"),
        (typeof(SampleOrphanController), nameof(ISampleOrphanAppService.GetAsync), "service interface not registered"),
        (typeof(SamplePingController), nameof(SamplePingController.Get), "no service interface")
    ];

    private AgentToolCatalog CreateSampleCatalog() =>
        CreateCatalog(o => o.IncludedRemoteServiceNames.Add(SampleRemoteServices.AgentToolsSample));

    private AgentToolDefinition Tool(string name)
    {
        var tool = CreateSampleCatalog().Find(name);
        tool.ShouldNotBeNull();
        return tool;
    }

    [Fact]
    public void Catalogue_Is_Empty_Without_Options()
    {
        var catalog = GetRequiredService<IAgentToolCatalog>();

        catalog.Tools.ShouldBeEmpty();
        catalog.Skipped.ShouldBeEmpty();
        catalog.Find("Book_GetList").ShouldBeNull();
    }

    [Fact]
    public void Including_A_Remote_Service_Exposes_Exactly_Its_Json_Input_Actions()
    {
        var catalog = CreateSampleCatalog();

        catalog.Tools.Select(t => t.Name).ShouldBe(AgentToolsSampleTools, ignoreOrder: true);
        catalog.Tools.ShouldAllBe(t => t.RemoteServiceName == SampleRemoteServices.AgentToolsSample);
    }

    [Theory]
    [InlineData("remote-service")]
    [InlineData("root-paths")]
    [InlineData("type-predicate")]
    public void Upload_Stream_Anonymous_Form_Disabled_Integration_And_Interfaceless_Actions_Are_Never_Exposed(string inclusion)
    {
        var catalog = CreateCatalog(o =>
        {
            switch (inclusion)
            {
                case "remote-service":
                    o.IncludedRemoteServiceNames.Add(SampleRemoteServices.AgentToolsSample);
                    break;
                case "root-paths":
                    o.IncludedRootPaths.Add("app");
                    o.IncludedRootPaths.Add(SampleRemoteServices.HandWrittenRootPath);
                    break;
                default:
                    // Every sample type (interface, or controller when it has none) except the duplicate-name one.
                    o.IncludedServiceTypes.Add(t => t.Namespace == typeof(IBookAppService).Namespace && t != typeof(IDuplicateNameAppService));
                    break;
            }
        });

        foreach (var (controller, action, reason) in NeverExposed)
        {
            catalog.Tools.ShouldNotContain(t => t.InterfaceMethod.Name == action && t.ServiceInterface.IsAssignableFrom(controller));
            catalog.Skipped.ShouldContain(
                s => s.ControllerType == controller.FullName && s.ActionName == action && s.Reason.StartsWith(reason),
                $"{controller.Name}.{action} should be skipped with '{reason}'");
        }

        catalog.Tools.Select(t => t.Name).ShouldBeSubsetOf(AgentToolsSampleTools.Concat(OtherSampleTools));
    }

    [Fact]
    public void Other_Remote_Service_Is_Not_Exposed_Unless_Included()
    {
        CreateSampleCatalog().Tools.ShouldNotContain(t => t.RemoteServiceName == SampleRemoteServices.OtherSample);

        var other = CreateCatalog(o => o.IncludedRemoteServiceNames.Add(SampleRemoteServices.OtherSample));

        other.Tools.Select(t => t.Name).ShouldBe(OtherSampleTools, ignoreOrder: true);
        other.Tools.ShouldAllBe(t => t.ServiceInterface == typeof(IOtherSampleAppService));
    }

    [Fact]
    public void Remote_Service_Names_Are_Case_Insensitive()
    {
        CreateCatalog(o => o.IncludedRemoteServiceNames.Add("agenttoolssample"))
            .Tools.Select(t => t.Name).ShouldBe(AgentToolsSampleTools, ignoreOrder: true);
    }

    [Fact]
    public void Root_Path_Include_Exposes_Only_That_Module()
    {
        var catalog = CreateCatalog(o => o.IncludedRootPaths.Add(SampleRemoteServices.OtherSampleRootPath));

        catalog.Tools.Select(t => t.Name).ShouldBe(OtherSampleTools, ignoreOrder: true);

        CreateCatalog(o => o.IncludedRootPaths.Add(SampleRemoteServices.HandWrittenRootPath))
            .Tools.Select(t => t.Name).ShouldBe(["SampleReport_Create", "SampleReport_Delete", "SampleReport_Get", "SampleReport_Get_2"], ignoreOrder: true);
    }

    [Fact]
    public void Type_Predicate_Include_Matches_The_Service_Interface()
    {
        var catalog = CreateCatalog(o => o.IncludedServiceTypes.Add(t => t == typeof(IBookAppService)));

        catalog.Tools.Select(t => t.Name).ShouldBe(AgentToolsSampleTools.Where(n => n.StartsWith("Book_")), ignoreOrder: true);

        // The implementation class is not the service type.
        CreateCatalog(o => o.IncludedServiceTypes.Add(t => t == typeof(BookAppService))).Tools.ShouldBeEmpty();
    }

    [Fact]
    public void Exclusion_Predicate_Wins_Over_Inclusion()
    {
        var catalog = CreateCatalog(o =>
        {
            o.IncludedRemoteServiceNames.Add(SampleRemoteServices.AgentToolsSample);
            o.IncludedServiceTypes.Add(t => t == typeof(IBookAppService));
            o.ExcludedServiceTypes.Add(t => t == typeof(IBookAppService));
        });

        catalog.Tools.Select(t => t.Name).ShouldBe(["SampleReport_Create", "SampleReport_Delete", "SampleReport_Get", "SampleReport_Get_2"], ignoreOrder: true);
        catalog.Skipped.ShouldContain(s => s.ControllerType == typeof(BookAppService).FullName
                                           && s.ActionName == nameof(IBookAppService.GetListAsync)
                                           && s.Reason.Contains("ExcludedServiceTypes"));
    }

    [Fact]
    public void Names_Strip_Async_And_Number_Overloads_In_Declaration_Order()
    {
        Tool("Book_GetList").ActionName.ShouldBe(nameof(IBookAppService.GetListAsync));

        var first = Tool("SampleReport_Get");
        var second = Tool("SampleReport_Get_2");
        first.InterfaceMethod.GetParameters().Single().ParameterType.ShouldBe(typeof(Guid));
        second.InterfaceMethod.GetParameters().Single().ParameterType.ShouldBe(typeof(string));
    }

    [Fact]
    public void Kinds_And_Annotations_Follow_The_Http_Verb()
    {
        var get = Tool("Book_Get");
        get.HttpMethod.ShouldBe("GET");
        get.Kind.ShouldBe(AgentToolKind.Read);
        (get.ReadOnly, get.Destructive, get.Idempotent, get.RequiresApprovalByDefault).ShouldBe((true, false, true, false));

        var create = Tool("Book_Create");
        create.HttpMethod.ShouldBe("POST");
        create.Kind.ShouldBe(AgentToolKind.Write);
        (create.ReadOnly, create.Destructive, create.Idempotent, create.RequiresApprovalByDefault).ShouldBe((false, false, false, true));

        var update = Tool("Book_Update");
        update.HttpMethod.ShouldBe("PUT");
        update.Kind.ShouldBe(AgentToolKind.Write);
        (update.ReadOnly, update.Destructive, update.Idempotent, update.RequiresApprovalByDefault).ShouldBe((false, false, true, true));

        var delete = Tool("Book_Delete");
        delete.HttpMethod.ShouldBe("DELETE");
        delete.Kind.ShouldBe(AgentToolKind.Destructive);
        (delete.ReadOnly, delete.Destructive, delete.Idempotent, delete.RequiresApprovalByDefault).ShouldBe((false, true, true, true));
    }

    [Fact]
    public void AgentTool_Attribute_Overrides_Name_Description_ReadOnly_Destructive_And_Approval()
    {
        var find = Tool("Book_Find");
        find.ActionName.ShouldBe(nameof(IBookAppService.SearchAsync));
        find.HttpMethod.ShouldBe("POST");
        find.Description.ShouldBe("Finds books whose name contains the given text.");
        find.Kind.ShouldBe(AgentToolKind.Read);
        (find.ReadOnly, find.Destructive, find.Idempotent, find.RequiresApprovalByDefault).ShouldBe((true, false, true, false));
        CreateSampleCatalog().Find("Book_Search").ShouldBeNull();

        var archive = Tool("Book_Archive");
        archive.HttpMethod.ShouldBe("POST");
        archive.Kind.ShouldBe(AgentToolKind.Destructive);
        (archive.ReadOnly, archive.Destructive, archive.RequiresApprovalByDefault).ShouldBe((false, true, false));

        var count = Tool("Book_GetCount");
        count.HttpMethod.ShouldBe("GET");
        count.Kind.ShouldBe(AgentToolKind.Read);
        count.RequiresApprovalByDefault.ShouldBeTrue();
    }

    [Fact]
    public void DisableAgentTool_Removes_The_Method()
    {
        var catalog = CreateSampleCatalog();

        catalog.Tools.ShouldNotContain(t => t.ActionName == nameof(IBookAppService.ResetAllAsync));
        catalog.Skipped.ShouldContain(s => s.ActionName == nameof(IBookAppService.ResetAllAsync) && s.Reason == "[DisableAgentTool]");
    }

    [Theory]
    [InlineData(typeof(BindingSampleAppService), nameof(IBindingSampleAppService.GetFromServicesAsync))]
    [InlineData(typeof(BindingSampleAppService), nameof(IBindingSampleAppService.GetFromKeyedServicesAsync))]
    [InlineData(typeof(BindingSampleAppService), nameof(IBindingSampleAppService.GetFromKeyedServicesOnTheInterfaceAsync))]
    [InlineData(typeof(BindingSampleAppService), nameof(IBindingSampleAppService.GetFromHeaderAsync))]
    [InlineData(typeof(BindingSampleAppService), nameof(IBindingSampleAppService.GetFromHeaderOnTheInterfaceAsync))]
    [InlineData(typeof(BindingSampleAppService), nameof(IBindingSampleAppService.GetWithHeaderMemberAsync))]
    [InlineData(typeof(BindingSampleReportController), nameof(IBindingSampleReportAppService.GetFromServicesAsync))]
    [InlineData(typeof(BindingSampleReportController), nameof(IBindingSampleReportAppService.GetFromInferredServicesAsync))]
    [InlineData(typeof(BindingSampleReportController), nameof(IBindingSampleReportAppService.GetFromServicesOnTheImplementationAsync))]
    public void An_Action_With_A_Parameter_From_Services_Or_A_Header_Is_Skipped(Type controller, string action)
    {
        var catalog = CreateCatalog(o => o.IncludedRemoteServiceNames.Add(BindingSampleAppService.RemoteServiceName));

        catalog.Tools.ShouldNotContain(t => t.InterfaceMethod.Name == action && t.ServiceInterface.IsAssignableFrom(controller));
        catalog.Skipped.Where(s => s.ControllerType == controller.FullName && s.ActionName == action)
            .ShouldHaveSingleItem().Reason.ShouldStartWith("input from services or a header");
    }

    [Fact]
    public void Route_Query_And_Body_Parameters_And_A_Cancellation_Token_Are_No_Reason_To_Skip()
    {
        CreateCatalog(o => o.IncludedRemoteServiceNames.Add(BindingSampleAppService.RemoteServiceName))
            .Tools.Select(t => t.Name).ShouldBe(
            [
                "BindingSample_Create", "BindingSample_Get", "BindingSample_GetPage",
                "BindingSampleReport_Create", "BindingSampleReport_Get"
            ], ignoreOrder: true);
    }

    [Fact]
    public void Description_Comes_From_Attribute_Then_Description_Then_Xml_Docs_Then_A_Generated_Sentence()
    {
        Tool("Book_Find").Description.ShouldBe("Finds books whose name contains the given text.");
        Tool("Book_GetCount").Description.ShouldBe("Counts the books of the current tenant.");
        Tool("Book_GetList").Description.ShouldBe("Lists the books of the current tenant.");
        Tool("Book_Get").Description.ShouldBe("Calls BookAppService.Get (GET api/app/book/{id}).");
    }

    [Fact]
    public void Parameters_Come_From_The_Interface_Method_With_Required_From_The_Model()
    {
        var update = Tool("Book_Update");

        update.Parameters.Select(p => (p.Name, p.Type, p.IsRequired)).ShouldBe([("id", typeof(Guid), true), ("input", typeof(UpdateBookDto), true)]);
        Tool("Book_GetList").Parameters.ShouldBeEmpty();
    }

    [Fact]
    public void Duplicate_Names_Fail_Fast()
    {
        var catalog = CreateCatalog(o =>
        {
            o.IncludedRemoteServiceNames.Add(SampleRemoteServices.AgentToolsSample);
            o.IncludedRemoteServiceNames.Add(SampleRemoteServices.DuplicateSample);
        });

        var exception = Should.Throw<AbpException>(() => catalog.Tools);

        exception.Message.ShouldContain("duplicate tool names");
        exception.Message.ShouldContain("'Book_GetList'");
        exception.Message.ShouldContain("IBookAppService.GetListAsync");
        exception.Message.ShouldContain("IDuplicateNameAppService.GetBookCountAsync");
        exception.Message.ShouldContain("[AgentTool(Name = ...)]");
    }

    [Fact]
    public void Names_That_Differ_Only_By_Case_Are_Duplicates()
    {
        var catalog = CreateCatalog(o => o.IncludedRemoteServiceNames.Add(SampleRemoteServices.DuplicateSample));

        var exception = Should.Throw<AbpException>(() => catalog.Tools);

        exception.Message.ShouldContain("duplicate tool names");
        exception.Message.ShouldContain("'Book_GetList' (IDuplicateNameAppService.GetBookCountAsync ", Case.Sensitive);
        exception.Message.ShouldContain("'book_getlist' (IDuplicateNameAppService.GetLowerCaseCountAsync ", Case.Sensitive);
    }

    [Fact]
    public void An_Overload_Suffix_Does_Not_Hide_A_Duplicate()
    {
        var catalog = CreateCatalog(o => o.IncludedServiceTypes.Add(t => t == typeof(IDuplicateNameAppService) || t == typeof(ISampleReportAppService)));

        var exception = Should.Throw<AbpException>(() => catalog.Tools);

        exception.Message.ShouldContain("'SampleReport_Get_2' (ISampleReportAppService.GetAsync ", Case.Sensitive);
        exception.Message.ShouldContain("'SAMPLEREPORT_GET_2' (IDuplicateNameAppService.GetReportCountAsync ", Case.Sensitive);
    }

    [Fact]
    public void Hand_Written_Controller_Is_Exposed_Through_Its_Service_Interface()
    {
        foreach (var name in new[] { "SampleReport_Create", "SampleReport_Get", "SampleReport_Get_2" })
        {
            var tool = Tool(name);
            tool.ServiceInterface.ShouldBe(typeof(ISampleReportAppService));
            tool.InterfaceMethod.DeclaringType.ShouldBe(typeof(ISampleReportAppService));
            tool.RemoteServiceName.ShouldBe(SampleRemoteServices.AgentToolsSample);
            tool.ModuleRootPath.ShouldBe(SampleRemoteServices.HandWrittenRootPath);
        }

        // Conventional controllers too: the interface, never the implementation class.
        Tool("Book_Get").ServiceInterface.ShouldBe(typeof(IBookAppService));
        Tool("Book_Get").InterfaceMethod.DeclaringType.ShouldBe(typeof(IBookAppService));
    }

    [Fact]
    public void Attributes_On_The_Implementation_Class_Behind_A_Hand_Written_Controller_Count()
    {
        var create = Tool("SampleReport_Create");
        create.ImplementationType.ShouldBe(typeof(SampleReportAppService));
        create.ImplementationMethod.DeclaringType.ShouldBe(typeof(SampleReportAppService));
        create.Description.ShouldBe("Creates a report."); // [AgentTool] on the implementation method
        Tool("SampleReport_Get_2").Description.ShouldBe("Gets a report by its code."); // [Description] on the implementation method

        CreateSampleCatalog().Skipped.ShouldContain(s => s.ControllerType == typeof(SampleReportController).FullName
                                                         && s.ActionName == nameof(ISampleReportAppService.GetLatestAsync)
                                                         && s.Reason == "[DisableAgentTool]");

        Tool("SampleReport_Delete").ControllerActionMethod!.DeclaringType.ShouldBe(typeof(SampleReportController));

        Tool("Book_Get").ImplementationType.ShouldBe(typeof(BookAppService)); // conventional: the controller class itself
        Tool("Book_Get").ControllerActionMethod.ShouldBeNull();
    }

    [Fact]
    public void Controller_Without_Interface_Is_Skipped_With_A_Logged_Reason()
    {
        var logger = new ListLogger<AgentToolCatalog>();
        var catalog = CreateCatalog(o => o.IncludedRemoteServiceNames.Add(SampleRemoteServices.AgentToolsSample), logger);

        catalog.Tools.ShouldNotContain(t => t.ControllerName == "SamplePing");
        var skipped = catalog.Skipped.Where(s => s.ControllerType == typeof(SamplePingController).FullName).ShouldHaveSingleItem();
        skipped.Reason.ShouldStartWith("no service interface");
        logger.Messages.ShouldContain(m => m.Contains(typeof(SamplePingController).FullName!) && m.Contains("no service interface"));
    }

    [Fact]
    public void MaxTools_Guard_Throws_With_A_Clear_Message()
    {
        var catalog = CreateCatalog(o =>
        {
            o.IncludedRemoteServiceNames.Add(SampleRemoteServices.AgentToolsSample);
            o.MaxTools = 5;
        });

        var exception = Should.Throw<AbpException>(() => catalog.Tools);

        exception.Message.ShouldContain($"{AgentToolsSampleTools.Length} actions are included as tools");
        exception.Message.ShouldContain("AbpAgentToolOptions.MaxTools is 5");
        exception.Message.ShouldContain("Include fewer remote services, root paths or service types");
        exception.Message.ShouldContain("raise AbpAgentToolOptions.MaxTools");
    }

    [Fact]
    public void MaxTools_Equal_To_The_Tool_Count_Is_Allowed()
    {
        CreateCatalog(o =>
        {
            o.IncludedRemoteServiceNames.Add(SampleRemoteServices.AgentToolsSample);
            o.MaxTools = AgentToolsSampleTools.Length;
        }).Tools.Count.ShouldBe(AgentToolsSampleTools.Length);
    }
}
