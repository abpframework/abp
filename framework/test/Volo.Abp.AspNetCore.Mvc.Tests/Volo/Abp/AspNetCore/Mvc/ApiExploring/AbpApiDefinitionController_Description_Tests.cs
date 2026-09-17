using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Http.Modeling;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring;

public class AbpApiDefinitionController_Description_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task Default_Should_Not_Include_Controller_Descriptions()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition");

        var controller = GetDocumentedController(model);
        controller.Summary.ShouldBeNull();
        controller.Remarks.ShouldBeNull();
        controller.Description.ShouldBeNull();
        controller.DisplayName.ShouldBeNull();
    }

    [Fact]
    public async Task Default_Should_Not_Include_Action_Descriptions()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition");

        var controller = GetDocumentedController(model);
        var action = GetAction(controller, "GetGreeting");
        action.Summary.ShouldBeNull();
        action.Remarks.ShouldBeNull();
        action.Description.ShouldBeNull();
        action.DisplayName.ShouldBeNull();
        action.ReturnValue.Summary.ShouldBeNull();
    }

    [Fact]
    public async Task Default_Should_Not_Include_Parameter_Descriptions()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition");

        var controller = GetDocumentedController(model);
        var action = GetAction(controller, "GetGreeting");

        var methodParam = action.ParametersOnMethod.FirstOrDefault(p => p.Name == "name");
        methodParam.ShouldNotBeNull();
        methodParam.Summary.ShouldBeNull();
        methodParam.Description.ShouldBeNull();
        methodParam.DisplayName.ShouldBeNull();

        var httpParam = action.Parameters.FirstOrDefault(p => p.NameOnMethod == "name");
        httpParam.ShouldNotBeNull();
        httpParam.Summary.ShouldBeNull();
    }

    [Fact]
    public async Task Default_Should_Not_Include_Type_Descriptions()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeTypes=true");

        var documentedDtoType = model.Types.FirstOrDefault(t => t.Key.Contains("DocumentedDto"));
        documentedDtoType.Value.ShouldNotBeNull();
        documentedDtoType.Value.Summary.ShouldBeNull();
        documentedDtoType.Value.Remarks.ShouldBeNull();
        documentedDtoType.Value.Description.ShouldBeNull();
        documentedDtoType.Value.DisplayName.ShouldBeNull();
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Populate_Controller_Summary()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var controller = GetDocumentedController(model);
        controller.Summary.ShouldNotBeNullOrEmpty();
        controller.Summary.ShouldContain("documented application service");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Populate_Controller_Remarks()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var controller = GetDocumentedController(model);
        controller.Remarks.ShouldNotBeNullOrEmpty();
        controller.Remarks.ShouldContain("integration tests");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Populate_Controller_Description_Attribute()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var controller = GetDocumentedController(model);
        controller.Description.ShouldBe("Documented service description from attribute");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Populate_Controller_DisplayName_Attribute()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var controller = GetDocumentedController(model);
        controller.DisplayName.ShouldBe("Documented Service");
    }

    [Fact]
    public async Task Controller_Descriptions_Should_Be_Populated_Only_Once_For_Multiple_Actions()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var controller = GetDocumentedController(model);

        controller.Actions.Count.ShouldBeGreaterThan(1);
        controller.Summary.ShouldNotBeNullOrEmpty();
        controller.Summary.ShouldContain("documented application service");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Populate_Action_Summary()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var action = GetAction(GetDocumentedController(model), "GetGreeting");
        action.Summary.ShouldNotBeNullOrEmpty();
        action.Summary.ShouldContain("greeting message");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Leave_Action_Remarks_Null_When_Not_Documented()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var action = GetAction(GetDocumentedController(model), "GetGreeting");
        action.Remarks.ShouldBeNull();
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Populate_Action_Description_Attribute()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var action = GetAction(GetDocumentedController(model), "GetGreeting");
        action.Description.ShouldBe("Get greeting description from attribute");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Populate_Action_DisplayName_Attribute()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var action = GetAction(GetDocumentedController(model), "GetGreeting");
        action.DisplayName.ShouldBe("Get Greeting");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Populate_ReturnValue_Summary()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var action = GetAction(GetDocumentedController(model), "GetGreeting");
        action.ReturnValue.Summary.ShouldNotBeNullOrEmpty();
        action.ReturnValue.Summary.ShouldContain("personalized greeting");
    }

    [Fact]
    public async Task Undocumented_Action_Should_Have_Null_Descriptions()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var action = GetAction(GetDocumentedController(model), "Delete");
        action.Summary.ShouldBeNull();
        action.Remarks.ShouldBeNull();
        action.Description.ShouldBeNull();
        action.DisplayName.ShouldBeNull();
        action.ReturnValue.Summary.ShouldBeNull();
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Populate_ParameterOnMethod_Summary()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var action = GetAction(GetDocumentedController(model), "GetGreeting");
        var param = action.ParametersOnMethod.FirstOrDefault(p => p.Name == "name");
        param.ShouldNotBeNull();
        param.Summary.ShouldNotBeNullOrEmpty();
        param.Summary.ShouldContain("name of the person");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Populate_ParameterOnMethod_Description_And_DisplayName_From_Attribute()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var action = GetAction(GetDocumentedController(model), "Search");
        var param = action.ParametersOnMethod.FirstOrDefault(p => p.Name == "query");
        param.ShouldNotBeNull();
        param.Summary.ShouldNotBeNullOrEmpty();
        param.Summary.ShouldContain("search query");
        param.Description.ShouldBe("Query param description from attribute");
        param.DisplayName.ShouldBe("Search Query");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Leave_Parameter_Attributes_Null_When_Not_Annotated()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var action = GetAction(GetDocumentedController(model), "Search");
        var param = action.ParametersOnMethod.FirstOrDefault(p => p.Name == "maxResults");
        param.ShouldNotBeNull();
        param.Summary.ShouldNotBeNullOrEmpty();
        param.Description.ShouldBeNull();
        param.DisplayName.ShouldBeNull();
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Populate_Parameter_Summary()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var action = GetAction(GetDocumentedController(model), "GetGreeting");
        var param = action.Parameters.FirstOrDefault(p => p.NameOnMethod == "name");
        param.ShouldNotBeNull();
        param.Summary.ShouldNotBeNullOrEmpty();
        param.Summary.ShouldContain("name of the person");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Populate_Parameter_Description_And_DisplayName_From_Attribute()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var action = GetAction(GetDocumentedController(model), "Search");
        var param = action.Parameters.FirstOrDefault(p => p.NameOnMethod == "query");
        param.ShouldNotBeNull();
        param.Summary.ShouldNotBeNullOrEmpty();
        param.Description.ShouldBe("Query param description from attribute");
        param.DisplayName.ShouldBe("Search Query");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Leave_Parameter_Attributes_Null_When_Not_Annotated_Http()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var action = GetAction(GetDocumentedController(model), "Search");
        var param = action.Parameters.FirstOrDefault(p => p.NameOnMethod == "maxResults");
        param.ShouldNotBeNull();
        param.Description.ShouldBeNull();
        param.DisplayName.ShouldBeNull();
    }

    [Fact]
    public async Task IncludeDescriptions_With_IncludeTypes_Should_Populate_Type_Summary()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true&includeTypes=true");

        var documentedDtoType = model.Types.FirstOrDefault(t => t.Key.Contains("DocumentedDto"));
        documentedDtoType.Value.ShouldNotBeNull();
        documentedDtoType.Value.Summary.ShouldNotBeNullOrEmpty();
        documentedDtoType.Value.Summary.ShouldContain("documented DTO");
    }

    [Fact]
    public async Task IncludeDescriptions_With_IncludeTypes_Should_Populate_Type_Description_And_DisplayName()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true&includeTypes=true");

        var documentedDtoType = model.Types.FirstOrDefault(t => t.Key.Contains("DocumentedDto"));
        documentedDtoType.Value.ShouldNotBeNull();
        documentedDtoType.Value.Description.ShouldBe("Documented DTO description from attribute");
        documentedDtoType.Value.DisplayName.ShouldBe("Documented DTO");
    }

    [Fact]
    public async Task IncludeDescriptions_With_IncludeTypes_Should_Populate_Property_Summary()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true&includeTypes=true");

        var documentedDtoType = model.Types.FirstOrDefault(t => t.Key.Contains("DocumentedDto"));
        documentedDtoType.Value.ShouldNotBeNull();
        documentedDtoType.Value.Properties.ShouldNotBeNull();

        var nameProp = documentedDtoType.Value.Properties!.FirstOrDefault(p => p.Name == "Name");
        nameProp.ShouldNotBeNull();
        nameProp.Summary.ShouldNotBeNullOrEmpty();
        nameProp.Summary.ShouldContain("name of the documented item");
    }

    [Fact]
    public async Task IncludeDescriptions_With_IncludeTypes_Should_Populate_Property_Description_And_DisplayName()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true&includeTypes=true");

        var documentedDtoType = model.Types.FirstOrDefault(t => t.Key.Contains("DocumentedDto"));
        documentedDtoType.Value.ShouldNotBeNull();

        var nameProp = documentedDtoType.Value.Properties!.FirstOrDefault(p => p.Name == "Name");
        nameProp.ShouldNotBeNull();
        nameProp.Description.ShouldBe("Name description from attribute");
        nameProp.DisplayName.ShouldBe("Item Name");
    }

    [Fact]
    public async Task IncludeDescriptions_With_IncludeTypes_Should_Leave_Property_DisplayName_Null_When_Not_Set()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true&includeTypes=true");

        var documentedDtoType = model.Types.FirstOrDefault(t => t.Key.Contains("DocumentedDto"));
        documentedDtoType.Value.ShouldNotBeNull();

        var valueProp = documentedDtoType.Value.Properties!.FirstOrDefault(p => p.Name == "Value");
        valueProp.ShouldNotBeNull();
        valueProp.Summary.ShouldNotBeNullOrEmpty();
        valueProp.Description.ShouldBe("Value description from attribute");
        valueProp.DisplayName.ShouldBeNull();
    }

    [Fact]
    public async Task IncludeTypes_Without_IncludeDescriptions_Should_Not_Populate_Type_Descriptions()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeTypes=true");

        var documentedDtoType = model.Types.FirstOrDefault(t => t.Key.Contains("DocumentedDto"));
        documentedDtoType.Value.ShouldNotBeNull();
        documentedDtoType.Value.Summary.ShouldBeNull();
        documentedDtoType.Value.Remarks.ShouldBeNull();
        documentedDtoType.Value.Description.ShouldBeNull();
        documentedDtoType.Value.DisplayName.ShouldBeNull();

        if (documentedDtoType.Value.Properties != null)
        {
            foreach (var prop in documentedDtoType.Value.Properties)
            {
                prop.Summary.ShouldBeNull();
                prop.Description.ShouldBeNull();
                prop.DisplayName.ShouldBeNull();
            }
        }
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Fallback_To_Interface_For_Controller_Summary()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var controller = GetInterfaceOnlyController(model);
        controller.Summary.ShouldNotBeNullOrEmpty();
        controller.Summary.ShouldContain("documented only on the interface");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Fallback_To_Interface_For_Controller_Remarks()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var controller = GetInterfaceOnlyController(model);
        controller.Remarks.ShouldNotBeNullOrEmpty();
        controller.Remarks.ShouldContain("resolved from the interface");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Fallback_To_Interface_For_Action_Summary()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var controller = GetInterfaceOnlyController(model);
        var action = GetAction(controller, "GetMessage");
        action.Summary.ShouldNotBeNullOrEmpty();
        action.Summary.ShouldContain("documented only on the interface");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Fallback_To_Interface_For_Action_ReturnValue_Summary()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var controller = GetInterfaceOnlyController(model);
        var action = GetAction(controller, "GetMessage");
        action.ReturnValue.Summary.ShouldNotBeNullOrEmpty();
        action.ReturnValue.Summary.ShouldContain("resolved message");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Fallback_To_Interface_For_Parameter_Summary()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var controller = GetInterfaceOnlyController(model);
        var action = GetAction(controller, "GetMessage");

        var methodParam = action.ParametersOnMethod.FirstOrDefault(p => p.Name == "key");
        methodParam.ShouldNotBeNull();
        methodParam.Summary.ShouldNotBeNullOrEmpty();
        methodParam.Summary.ShouldContain("message key");

        var httpParam = action.Parameters.FirstOrDefault(p => p.NameOnMethod == "key");
        httpParam.ShouldNotBeNull();
        httpParam.Summary.ShouldNotBeNullOrEmpty();
        httpParam.Summary.ShouldContain("message key");
    }

    [Fact]
    public async Task IncludeDescriptions_Should_Not_Apply_Container_Param_Summary_To_Expanded_Properties()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition?includeDescriptions=true");

        var controller = GetDocumentedController(model);
        var action = GetAction(controller, "Create");

        // Expanded properties from DocumentedDto should not have the container parameter's summary
        var expandedParams = action.Parameters
            .Where(p => !string.IsNullOrEmpty(p.DescriptorName) && p.Name != p.NameOnMethod)
            .ToList();

        foreach (var param in expandedParams)
        {
            param.Summary.ShouldBeNull();
            param.Description.ShouldBeNull();
            param.DisplayName.ShouldBeNull();
        }
    }

    [Fact]
    public async Task Action_ImplementFrom_Should_Point_To_Implemented_Interface()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition");

        var controller = GetDocumentedController(model);
        var action = GetAction(controller, "GetGreeting");

        action.ImplementFrom.ShouldNotBeNullOrEmpty();
        action.ImplementFrom.ShouldContain("IDocumentedAppService");
        action.ImplementFrom.ShouldNotContain("DocumentedAppService.");
    }

    [Fact]
    public async Task Action_ImplementFrom_Should_Point_To_Interface_When_Only_Documented_On_Interface()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>(
            "/api/abp/api-definition");

        var controller = GetInterfaceOnlyController(model);
        var action = GetAction(controller, "GetMessage");

        action.ImplementFrom.ShouldNotBeNullOrEmpty();
        action.ImplementFrom.ShouldContain("IInterfaceOnlyDocumentedAppService");
        action.ImplementFrom.ShouldNotContain("InterfaceOnlyDocumentedAppService.");
    }

    [Fact]
    public void CreateSubModel_Should_Preserve_All_Controller_Properties()
    {
        var controller = ControllerApiDescriptionModel.Create(
            "TestController",
            "TestGroup",
            isRemoteService: true,
            isIntegrationService: false,
            apiVersion: "1.0",
            typeof(AbpApiDefinitionController_Description_Tests));

        controller.Summary = "Test summary";
        controller.Remarks = "Test remarks";
        controller.Description = "Test description";
        controller.DisplayName = "Test display name";

        var subModel = controller.CreateSubModel(null);

        subModel.ControllerName.ShouldBe("TestController");
        subModel.ControllerGroupName.ShouldBe("TestGroup");
        subModel.IsRemoteService.ShouldBeTrue();
        subModel.IsIntegrationService.ShouldBeFalse();
        subModel.ApiVersion.ShouldBe("1.0");
        subModel.Summary.ShouldBe("Test summary");
        subModel.Remarks.ShouldBe("Test remarks");
        subModel.Description.ShouldBe("Test description");
        subModel.DisplayName.ShouldBe("Test display name");
        subModel.Type.ShouldBe(controller.Type);
    }

    private static ControllerApiDescriptionModel GetDocumentedController(ApplicationApiDescriptionModel model)
    {
        return model.Modules.Values
            .SelectMany(m => m.Controllers.Values)
            .First(c => c.ControllerName == "Documented");
    }

    private static ControllerApiDescriptionModel GetInterfaceOnlyController(ApplicationApiDescriptionModel model)
    {
        return model.Modules.Values
            .SelectMany(m => m.Controllers.Values)
            .First(c => c.ControllerName == "InterfaceOnlyDocumented");
    }

    private static ActionApiDescriptionModel GetAction(ControllerApiDescriptionModel controller, string actionName)
    {
        return controller.Actions.Values
            .First(a => a.Name == actionName + "Async" || a.Name == actionName);
    }
}
