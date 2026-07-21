using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Http.Modeling;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring;

public class AbpApiDefinitionController_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task GetAsync()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>("/api/abp/api-definition");
        model.ShouldNotBeNull();
        model.Types.IsNullOrEmpty().ShouldBeTrue();
    }

    [Fact]
    public async Task GetAsync_IncludeTypes()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>("/api/abp/api-definition?includeTypes=true");
        model.ShouldNotBeNull();
        model.Types.IsNullOrEmpty().ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Have_Null_AllowAnonymous_For_Actions_Without_Authorization()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>("/api/abp/api-definition");

        var peopleController = GetPeopleController(model);
        var action = GetAction(peopleController, "GetPhones");

        action.AllowAnonymous.ShouldBeNull();
        action.AuthorizeDatas.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Set_AllowAnonymous_True_For_AllowAnonymous_Actions()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>("/api/abp/api-definition");

        var peopleController = GetPeopleController(model);
        var action = GetAction(peopleController, "GetWithAllowAnonymous");

        action.AllowAnonymous.ShouldBe(true);
        action.AuthorizeDatas.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Set_AllowAnonymous_False_And_AuthorizeDatas_For_Authorize_Actions()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>("/api/abp/api-definition");

        var peopleController = GetPeopleController(model);
        var action = GetAction(peopleController, "GetWithAuthorized");

        action.AllowAnonymous.ShouldBe(false);
        action.AuthorizeDatas.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Should_Contain_Policy_And_Roles_In_AuthorizeDatas()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>("/api/abp/api-definition");

        var peopleController = GetPeopleController(model);
        var action = GetAction(peopleController, "GetWithAuthorizePolicy");

        action.AllowAnonymous.ShouldBe(false);
        action.AuthorizeDatas.Count.ShouldBe(2);
        action.AuthorizeDatas.ShouldContain(a => a.Policy == "TestPolicy" && a.Roles == "Admin");
        action.AuthorizeDatas.ShouldContain(a => a.Policy == "TestPolicy2" && a.Roles == "Manager");
    }

    private static ControllerApiDescriptionModel GetPeopleController(ApplicationApiDescriptionModel model)
    {
        return model.Modules.Values
            .SelectMany(m => m.Controllers.Values)
            .First(c => c.ControllerName == "People");
    }

    private static ActionApiDescriptionModel GetAction(ControllerApiDescriptionModel controller, string actionName)
    {
        return controller.Actions.Values
            .First(a => a.Name == actionName + "Async" || a.Name == actionName);
    }
}
