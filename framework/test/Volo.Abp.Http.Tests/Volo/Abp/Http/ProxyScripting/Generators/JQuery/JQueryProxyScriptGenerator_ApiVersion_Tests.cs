#nullable enable
using System.Collections.Generic;
using Shouldly;
using Volo.Abp.Http.Modeling;
using Xunit;

namespace Volo.Abp.Http.ProxyScripting.Generators.JQuery;

public class JQueryProxyScriptGenerator_ApiVersion_Tests
{
    private readonly JQueryProxyScriptGenerator _generator = new(
        Microsoft.Extensions.Options.Options.Create(new DynamicJavaScriptProxyOptions()));

    [Fact]
    public void Should_Default_Query_String_Version_Of_Simple_Parameters()
    {
        var script = _generator.CreateScript(BuildAppModel("api/app/book/{id}",
            ("id", "id", ParameterBindingSources.Path),
            ("api-version", "api-version", ParameterBindingSources.Query)));

        script.ShouldContain("function(id, api_version, ajaxParams)");
        script.ShouldContain("api_version = api_version ? api_version : '2.0';");
        script.ShouldContain("{ name: 'api-version', value: api_version }");
    }

    [Fact]
    public void Should_Default_Query_String_Version_Of_Input_Dto()
    {
        var script = _generator.CreateScript(BuildAppModel("api/app/book",
            ("Filter", "input", ParameterBindingSources.ModelBinding),
            ("api-version", "input", ParameterBindingSources.Query)));

        script.ShouldContain("function(input, ajaxParams)");
        script.ShouldContain("var apiVersionContainer = { api_version: input.api_version ? input.api_version : '2.0' };");
        script.ShouldContain("{ name: 'api-version', value: apiVersionContainer.api_version }");
        script.ShouldNotContain("input.api_version =");
        script.ShouldNotContain("input = ");
    }

    [Fact]
    public void Should_Default_Url_Segment_Version_Of_Simple_Parameters()
    {
        var script = _generator.CreateScript(BuildAppModel("api/v{apiVersion}/book/{id}",
            ("id", "id", ParameterBindingSources.Path),
            ("apiVersion", "apiVersion", ParameterBindingSources.Path)));

        script.ShouldContain("function(id, apiVersion, ajaxParams)");
        script.ShouldContain("apiVersion = apiVersion ? apiVersion : '2.0';");
        script.ShouldContain("'api/v' + apiVersion + '/book/' + id");
    }

    [Fact]
    public void Should_Default_Url_Segment_Version_Of_Input_Dto()
    {
        var script = _generator.CreateScript(BuildAppModel("api/v{apiVersion}/book",
            ("Filter", "input", ParameterBindingSources.Query),
            ("apiVersion", "input", ParameterBindingSources.Path)));

        script.ShouldContain("function(input, ajaxParams)");
        script.ShouldContain("var apiVersionContainer = { apiVersion: input.apiVersion ? input.apiVersion : '2.0' };");
        script.ShouldContain("'api/v' + apiVersionContainer.apiVersion + '/book'");
        script.ShouldNotContain("input.apiVersion =");
        script.ShouldNotContain("input = ");
    }

    [Fact]
    public void Should_Default_Query_String_Version_Without_Other_Parameters()
    {
        var script = _generator.CreateScript(BuildAppModel("api/app/book/count",
            ("api-version", "api-version", ParameterBindingSources.Query)));

        script.ShouldContain("function(api_version, ajaxParams)");
        script.ShouldContain("api_version = api_version ? api_version : '2.0';");
        script.ShouldContain("'api/app/book/count' + abp.utils.buildQueryString([{ name: 'api-version', value: api_version }])");
    }

    [Fact]
    public void Should_Default_Query_String_Version_Of_Body_Parameter()
    {
        var script = _generator.CreateScript(BuildAppModel("api/app/book", "POST", new List<string> { "2.0" },
            ("input", "input", ParameterBindingSources.Body),
            ("api-version", "api-version", ParameterBindingSources.Query)));

        script.ShouldContain("function(input, api_version, ajaxParams)");
        script.ShouldContain("api_version = api_version ? api_version : '2.0';");
        script.ShouldContain("data: JSON.stringify(input)");
    }

    [Fact]
    public void Should_Default_To_Version_1_When_There_Are_No_Supported_Versions()
    {
        var script = _generator.CreateScript(BuildAppModel("api/v{apiVersion}/book", "GET", new List<string>(),
            ("Filter", "input", ParameterBindingSources.Query),
            ("apiVersion", "input", ParameterBindingSources.Path)));

        script.ShouldContain("var apiVersionContainer = { apiVersion: input.apiVersion ? input.apiVersion : '1.0' };");
    }

    [Fact]
    public void Should_Keep_The_Form_Data_Of_A_Versioned_Upload()
    {
        var script = _generator.CreateScript(BuildAppModel("api/app/book/upload", "POST", new List<string> { "2.0" },
            ("Name", "input", ParameterBindingSources.Form),
            ("File", "input", ParameterBindingSources.FormFile),
            ("api-version", "input", ParameterBindingSources.Query)));

        script.ShouldContain("function(input, ajaxParams)");
        script.ShouldContain("var apiVersionContainer = { api_version: input.api_version ? input.api_version : '2.0' };");
        script.ShouldContain("'api/app/book/upload' + abp.utils.buildQueryString([{ name: 'api-version', value: apiVersionContainer.api_version }])");
        script.ShouldContain("data: input,");
        script.ShouldNotContain("input = ");
    }

    [Fact]
    public void Should_Not_Overwrite_A_Method_Parameter_Named_Like_The_Version()
    {
        var script = _generator.CreateScript(BuildAppModel("api/app/book",
            ("api_version", "api_version", ParameterBindingSources.ModelBinding),
            ("Filter", "input", ParameterBindingSources.ModelBinding),
            ("api-version", "input", ParameterBindingSources.Query)));

        script.ShouldContain("function(api_version, input, ajaxParams)");
        script.ShouldContain("var apiVersionContainer = { api_version: input.api_version ? input.api_version : '2.0' };");
        script.ShouldContain("{ name: 'api_version', value: api_version }");
        script.ShouldContain("{ name: 'api-version', value: apiVersionContainer.api_version }");
        script.ShouldNotContain("var api_version");
    }

    [Fact]
    public void Should_Not_Overwrite_A_Method_Parameter_Named_Like_The_Version_Container()
    {
        var script = _generator.CreateScript(BuildAppModel("api/app/book",
            ("apiVersionContainer", "apiVersionContainer", ParameterBindingSources.ModelBinding),
            ("Filter", "input", ParameterBindingSources.ModelBinding),
            ("api-version", "input", ParameterBindingSources.Query)));

        script.ShouldContain("function(apiVersionContainer, input, ajaxParams)");
        script.ShouldContain("var apiVersionContainer_ = { api_version: input.api_version ? input.api_version : '2.0' };");
        script.ShouldContain("{ name: 'apiVersionContainer', value: apiVersionContainer }");
        script.ShouldContain("{ name: 'api-version', value: apiVersionContainer_.api_version }");
    }

    private static ApplicationApiDescriptionModel BuildAppModel(string url,
        params (string Name, string NameOnMethod, string BindingSourceId)[] parameters)
    {
        return BuildAppModel(url, "GET", new List<string> { "1.0", "2.0" }, parameters);
    }

    private static ApplicationApiDescriptionModel BuildAppModel(string url, string httpMethod, List<string> supportedVersions,
        params (string Name, string NameOnMethod, string BindingSourceId)[] parameters)
    {
        var model = ApplicationApiDescriptionModel.Create();
        var module = model.GetOrAddModule("app", "Default");
        var controller = module.GetOrAddController(
            name: "Book",
            groupName: null,
            isRemoteService: true,
            isIntegrationService: false,
            apiVersion: null,
            type: typeof(object));

        var parameterModels = new List<ParameterApiDescriptionModel>();
        foreach (var (name, nameOnMethod, bindingSourceId) in parameters)
        {
            parameterModels.Add(new ParameterApiDescriptionModel
            {
                Name = name,
                NameOnMethod = nameOnMethod,
                Type = "System.String",
                TypeSimple = "string",
                BindingSourceId = bindingSourceId,
                DescriptorName = name == nameOnMethod || name is "api-version" or "apiVersion" ? string.Empty : nameOnMethod
            });
        }

        var action = new ActionApiDescriptionModel
        {
            UniqueName = "GetAsync",
            Name = "GetAsync",
            HttpMethod = httpMethod,
            Url = url,
            SupportedVersions = supportedVersions,
            ParametersOnMethod = new List<MethodParameterApiDescriptionModel>(),
            Parameters = parameterModels,
            ReturnValue = new ReturnValueApiDescriptionModel
            {
                Type = "System.String",
                TypeSimple = "string",
            },
            AuthorizeDatas = new List<AuthorizeDataApiDescriptionModel>(),
        };
        controller.AddAction("GetAsync", action);

        return model;
    }
}
