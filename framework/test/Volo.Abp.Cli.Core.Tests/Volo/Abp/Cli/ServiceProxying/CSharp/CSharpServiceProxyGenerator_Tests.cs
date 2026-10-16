using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json.SystemTextJson;
using Xunit;

namespace Volo.Abp.Cli.ServiceProxying.CSharp;

public class CSharpServiceProxyGenerator_Tests : IDisposable
{
    private const string OrderAppService = "MyCompany.Orders.Application.IOrderAppService";

    private static readonly string[] DefaultClientAssemblyNames =
    {
        "Volo.Abp.Core",
        "Volo.Abp.Ddd.Application.Contracts",
        "Volo.Abp.Http",
        "Volo.Abp.Http.Abstractions",
        "Volo.Abp.MultiTenancy.Abstractions",
        "Volo.Abp.ObjectExtending",
        "Volo.Abp.Validation.Abstractions"
    };

    private readonly TestCSharpServiceProxyGenerator _generator = new();
    private readonly string _workDirectory;

    public CSharpServiceProxyGenerator_Tests()
    {
        _workDirectory = Path.Combine(Path.GetTempPath(), "abp-csharp-proxy-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_workDirectory);
        File.WriteAllText(Path.Combine(_workDirectory, "Client.csproj"), "<Project Sdk=\"Microsoft.NET.Sdk\" />");
        WriteProjectAssets((".NETCoreApp,Version=v10.0", DefaultClientAssemblyNames));
        _generator.WorkDirectory = _workDirectory;
        _generator.ProjectAssetsFilePath = Path.Combine(_workDirectory, "obj", "project.assets.json");
    }

    public void Dispose()
    {
        Directory.Delete(_workDirectory, true);
    }

    [Fact]
    public void Should_Generate_Types_Used_By_Service_Signatures_From_Any_Namespace()
    {
        var model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetAsync", "MyCompany.Orders.OrderDto", "System.Guid"),
                CreateMethod("GetListAsync", "Volo.Abp.Application.Dtos.PagedResultDto<MyCompany.Orders.OrderDto>", "MyCompany.Orders.GetOrdersInput"),
                CreateMethod("GetStatusesAsync", "[MyCompany.Shared.OrderStatus]"),
                CreateMethod("GetCustomersAsync", "{System.String:MyCompany.Shared.CustomerDto}")));

        _generator.GetTypes(model).ShouldBe(new[]
        {
            "MyCompany.Orders.GetOrdersInput",
            "MyCompany.Orders.OrderDto",
            "MyCompany.Orders.OrderLineDto",
            "MyCompany.Shared.AddressDto",
            "MyCompany.Shared.CustomerDto",
            "MyCompany.Shared.OrderStatus"
        });
    }

    [Fact]
    public void Should_Generate_Types_Referenced_By_Base_Types_Properties_And_Generic_Arguments()
    {
        var model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetAsync", "MyCompany.Orders.OrderDetailDto")));

        _generator.GetTypes(model).ShouldBe(new[]
        {
            "MyCompany.Orders.OrderDetailDto",
            "MyCompany.Orders.OrderDto",
            "MyCompany.Orders.OrderLineDto",
            "MyCompany.Shared.AddressDto",
            "MyCompany.Shared.CustomerDto",
            "MyCompany.Shared.OrderStatus",
            "MyCompany.Shared.TagDto",
            "MyCompany.Shared.Wrapper<T0>"
        });
    }

    [Fact]
    public void Should_Match_Generic_Types_By_Generic_Argument_Count()
    {
        var model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetAsync", "MyCompany.Shared.Result<MyCompany.Shared.Wrapper<MyCompany.Shared.TagDto>,MyCompany.Shared.AddressDto>")));

        _generator.GetTypes(model).ShouldBe(new[]
        {
            "MyCompany.Shared.AddressDto",
            "MyCompany.Shared.Result<T0,T1>",
            "MyCompany.Shared.TagDto",
            "MyCompany.Shared.Wrapper<T0>"
        });
    }

    [Fact]
    public void Should_Not_Generate_Types_That_Only_Share_The_Service_Namespace()
    {
        var model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetStatusesAsync", "[MyCompany.Shared.OrderStatus]")));

        var types = _generator.GetTypes(model);

        types.ShouldNotContain("MyCompany.Orders.Application.UnusedDto");
        types.ShouldBe(new[] { "MyCompany.Shared.OrderStatus" });
    }

    [Fact]
    public void Should_Not_Generate_Types_Provided_By_The_Framework()
    {
        var model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetSettingsAsync", "[Volo.Abp.NameValue]"),
                CreateMethod("UploadAsync", "System.Void", "Volo.Abp.Content.IRemoteStreamContent"),
                CreateMethod("GetValueTypeAsync", "Volo.Abp.Validation.StringValues.IStringValueType"),
                CreateMethod("GetApiDefinitionAsync", "Volo.Abp.Http.Modeling.ApplicationApiDescriptionModel"),
                CreateMethod("UploadFileAsync", "System.Void", "Microsoft.AspNetCore.Http.IFormFile"),
                CreateMethod("GetExtensibleAsync", "Volo.Abp.ObjectExtending.ExtensibleObject"),
                CreateMethod("GetListAsync", "Volo.Abp.Application.Dtos.ListResultDto<Volo.Abp.Application.Dtos.EntityDto>")));

        _generator.GetTypes(model).ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Generate_Abp_Module_Contract_Types_But_Not_Framework_Types()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetUserAsync", "Volo.Abp.Users.UserData"),
                CreateMethod("GetTenantAsync", "Volo.Abp.MultiTenancy.TenantConfiguration"),
                CreateMethod("GetSettingsAsync", "Volo.Abp.NameValue")));

        await _generator.GenerateProxyAsync(CreateArgs());

        var userDataFile = GetProxyFilePath("Volo/Abp/Users/UserData.cs");
        File.Exists(userDataFile).ShouldBeTrue();
        (await File.ReadAllTextAsync(userDataFile)).ShouldContain("public class UserData");
        File.Exists(GetProxyFilePath("Volo/Abp/MultiTenancy/TenantConfiguration.cs")).ShouldBeFalse();
        File.Exists(GetProxyFilePath("Volo/Abp/NameValue.cs")).ShouldBeFalse();
    }

    [Fact]
    public void Should_Generate_Types_Of_The_Targeted_Abp_Module()
    {
        var model = CreateModel(
            CreateServiceController("Volo.Abp.Identity.Integration.IIdentityUserIntegrationService",
                CreateMethod("SearchAsync", "Volo.Abp.Identity.UserLookupSearchInputDto"),
                CreateMethod("GetUserAsync", "Volo.Abp.Users.UserData"),
                CreateMethod("GetTenantAsync", "Volo.Abp.MultiTenancy.TenantConfiguration")));

        _generator.GetTypes(model).ShouldBe(new[]
        {
            "Volo.Abp.Identity.UserLookupSearchInputDto",
            "Volo.Abp.Users.UserData"
        });
    }

    [Fact]
    public void Should_Generate_Cross_Module_Contracts_Not_Provided_By_Client_Packages()
    {
        var model = CreateModel(
            CreateServiceController("Volo.Abp.Account.IAccountAppService",
                CreateMethod("RegisterAsync", "Volo.Abp.Identity.IdentityUserDto"),
                CreateMethod("GetTenantAsync", "Volo.Abp.MultiTenancy.TenantConfiguration")));

        _generator.GetTypes(model).ShouldBe(new[]
        {
            "Volo.Abp.Identity.IdentityUserDto"
        });
    }

    [Fact]
    public void Should_Not_Generate_Cross_Module_Contracts_Provided_By_All_Target_Frameworks()
    {
        WriteProjectAssets(
            (".NETCoreApp,Version=v9.0", DefaultClientAssemblyNames.Append("Volo.Abp.Identity.Application.Contracts").ToArray()),
            (".NETCoreApp,Version=v10.0", DefaultClientAssemblyNames.Append("Volo.Abp.Identity.Application.Contracts").ToArray()));
        var model = CreateModel(
            CreateServiceController("Volo.Abp.Account.IAccountAppService",
                CreateMethod("RegisterAsync", "Volo.Abp.Identity.IdentityUserDto")));

        _generator.GetTypes(model).ShouldBeEmpty();
    }

    [Fact]
    public void Should_Generate_Contracts_Not_Provided_By_Every_Target_Framework()
    {
        WriteProjectAssets(
            (".NETCoreApp,Version=v9.0", DefaultClientAssemblyNames.Append("Volo.Abp.Identity.Application.Contracts").ToArray()),
            (".NETCoreApp,Version=v10.0", DefaultClientAssemblyNames));
        var model = CreateModel(
            CreateServiceController("Volo.Abp.Account.IAccountAppService",
                CreateMethod("RegisterAsync", "Volo.Abp.Identity.IdentityUserDto")));

        _generator.GetTypes(model).ShouldBe(new[]
        {
            "Volo.Abp.Identity.IdentityUserDto"
        });
    }

    [Fact]
    public void Should_Use_Namespace_Fallback_When_Assembly_Names_Are_Missing()
    {
        var model = CreateModel(
            CreateServiceController("Volo.Abp.Identity.Integration.IIdentityUserIntegrationService",
                CreateMethod("SearchAsync", "Volo.Abp.Identity.UserLookupSearchInputDto"),
                CreateMethod("GetUserAsync", "Volo.Abp.Users.UserData"),
                CreateMethod("GetTenantAsync", "Volo.Abp.MultiTenancy.TenantConfiguration")));
        foreach (var type in model.Types.Values)
        {
            type.AssemblyName = null;
        }

        _generator.GetTypes(model).ShouldBe(new[]
        {
            "Volo.Abp.Identity.UserLookupSearchInputDto",
            "Volo.Abp.Users.UserData"
        });
    }

    [Fact]
    public void Should_Use_Namespace_Fallback_When_Project_Assets_Are_Missing()
    {
        File.Delete(Path.Combine(_workDirectory, "obj", "project.assets.json"));
        var model = CreateModel(
            CreateServiceController("Volo.Abp.Identity.Integration.IIdentityUserIntegrationService",
                CreateMethod("SearchAsync", "Volo.Abp.Identity.UserLookupSearchInputDto"),
                CreateMethod("GetUserAsync", "Volo.Abp.Users.UserData"),
                CreateMethod("GetTenantAsync", "Volo.Abp.MultiTenancy.TenantConfiguration")));

        _generator.GetTypes(model).ShouldBe(new[]
        {
            "Volo.Abp.Identity.UserLookupSearchInputDto",
            "Volo.Abp.Users.UserData"
        });
    }

    [Fact]
    public void Should_Use_Namespace_Fallback_When_Project_Assets_Are_Invalid()
    {
        File.WriteAllText(
            Path.Combine(_workDirectory, "obj", "project.assets.json"),
            "{\"version\":3,\"targets\":null}");
        var model = CreateModel(
            CreateServiceController("Volo.Abp.Identity.Integration.IIdentityUserIntegrationService",
                CreateMethod("SearchAsync", "Volo.Abp.Identity.UserLookupSearchInputDto"),
                CreateMethod("GetUserAsync", "Volo.Abp.Users.UserData"),
                CreateMethod("GetTenantAsync", "Volo.Abp.MultiTenancy.TenantConfiguration")));

        _generator.GetTypes(model).ShouldBe(new[]
        {
            "Volo.Abp.Identity.UserLookupSearchInputDto",
            "Volo.Abp.Users.UserData"
        });
    }

    [Fact]
    public void Should_Use_Project_Assets_From_A_Custom_Path()
    {
        File.Delete(Path.Combine(_workDirectory, "obj", "project.assets.json"));
        var customAssetsFilePath = Path.Combine(_workDirectory, "artifacts", "obj", "project.assets.json");
        File.WriteAllText(
            Path.Combine(_workDirectory, "Directory.Build.props"),
            "<Project><PropertyGroup><BaseIntermediateOutputPath>artifacts/obj/</BaseIntermediateOutputPath></PropertyGroup></Project>");
        WriteProjectAssets(customAssetsFilePath,
            (".NETCoreApp,Version=v10.0", DefaultClientAssemblyNames));
        _generator.ProjectAssetsFilePath = null;
        var model = CreateModel(
            CreateServiceController("Volo.Abp.Account.IAccountAppService",
                CreateMethod("RegisterAsync", "Volo.Abp.Identity.IdentityUserDto")));

        _generator.GetTypes(model).ShouldBe(new[] { "Volo.Abp.Identity.IdentityUserDto" });
    }

    [Fact]
    public async Task Should_Not_Read_Project_Assets_Without_Contracts()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetAsync", "MyCompany.Orders.OrderDto")));

        await _generator.GenerateProxyAsync(CreateArgs(withoutContracts: true));

        _generator.ProjectAssetsFilePathCallCount.ShouldBe(0);
    }

    [Fact]
    public void Should_Not_Generate_Types_Of_The_Abp_Framework_Module()
    {
        var model = CreateModuleModel("abp",
            CreateServiceController("Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.IAbpApplicationConfigurationAppService",
                CreateMethod("GetAsync", "Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ApplicationConfigurationDto")));
        AddType(model, "Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ApplicationConfigurationDto", null);

        _generator.GetTypes(model).ShouldBeEmpty();
    }

    [Fact]
    public void Should_Not_Generate_Types_Of_Controllers_Without_A_Service_Interface()
    {
        var accountController = new ControllerApiDescriptionModel
        {
            ControllerName = "Account",
            Type = "MyCompany.Orders.Web.AccountController",
            Interfaces = new List<ControllerInterfaceApiDescriptionModel>(),
            Actions = new Dictionary<string, ActionApiDescriptionModel>
            {
                ["LoginAsync"] = new()
                {
                    Name = "LoginAsync",
                    ImplementFrom = "MyCompany.Orders.Web.AccountController",
                    ParametersOnMethod = new List<MethodParameterApiDescriptionModel>(),
                    ReturnValue = new ReturnValueApiDescriptionModel { Type = "MyCompany.Orders.Web.LoginResult" }
                }
            }
        };

        var model = CreateModel(
            accountController,
            CreateServiceController(OrderAppService,
                CreateMethod("GetStatusesAsync", "[MyCompany.Shared.OrderStatus]")));

        _generator.GetTypes(model).ShouldBe(new[] { "MyCompany.Shared.OrderStatus" });
    }

    [Fact]
    public async Task Should_Generate_Types_Used_By_Action_Parameters()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("Search", "System.Int32")));
        _generator.Model.Modules["orders"].Controllers.Values.Single().Actions["Search"].Parameters.Add(
            new ParameterApiDescriptionModel
            {
                Name = "status",
                NameOnMethod = "status",
                Type = "MyCompany.Shared.OrderStatus"
            });

        await _generator.GenerateProxyAsync(CreateArgs());

        File.Exists(GetProxyFilePath("MyCompany/Shared/OrderStatus.cs")).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Generate_Generic_Type_Properties_With_Their_Own_Types()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetAsync", "MyCompany.Shared.Wrapper<MyCompany.Shared.TagDto>")));

        await _generator.GenerateProxyAsync(CreateArgs());

        var wrapper = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Shared/Wrapper.cs"));
        wrapper.ShouldContain("public class Wrapper<T>");
        wrapper.ShouldContain("public T Value { get; set; }");
        wrapper.ShouldContain("public T[] Items { get; set; }");
    }

    [Fact]
    public async Task Should_Generate_Same_Named_Generic_Types_Into_Separate_Files()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetAsync", "MyCompany.Shared.Result"),
                CreateMethod("GetTagAsync", "MyCompany.Shared.Result<MyCompany.Shared.TagDto>"),
                CreateMethod("GetTagAndAddressAsync", "MyCompany.Shared.Result<MyCompany.Shared.TagDto,MyCompany.Shared.AddressDto>")));

        await _generator.GenerateProxyAsync(CreateArgs());

        (await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Shared/Result.cs"))).ShouldContain($"public class Result{Environment.NewLine}");
        (await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Shared/Result{T}.cs"))).ShouldContain("public class Result<T>");
        (await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Shared/Result{TItem,TError}.cs"))).ShouldContain("public class Result<TItem, TError>");
    }

    [Fact]
    public async Task Should_Add_Usings_For_Generic_Types_From_Other_Namespaces()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetAsync", "MyCompany.Generic.Box<MyCompany.Shared.TagDto>"),
                CreateMethod("GetBoxedAsync", "MyCompany.Orders.BoxedOrderDto")));

        await _generator.GenerateProxyAsync(CreateArgs());

        var serviceInterface = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/Application/IOrderAppService.cs"));
        serviceInterface.ShouldContain("using MyCompany.Generic;");
        serviceInterface.ShouldContain("using MyCompany.Shared;");
        serviceInterface.ShouldContain("Task<Box<TagDto>> GetAsync()");

        var boxedOrder = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/BoxedOrderDto.cs"));
        boxedOrder.ShouldContain("using MyCompany.Generic;");
        boxedOrder.ShouldContain("using MyCompany.Shared;");
        boxedOrder.ShouldContain("public Box<Box<TagDto>> Nested { get; set; }");
    }

    [Fact]
    public async Task Should_Keep_Service_Interfaces_When_No_Type_Is_Returned_By_The_Server()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetCountAsync", "System.Int32")));
        _generator.Model.Types.Clear();

        await _generator.GenerateProxyAsync(CreateArgs());

        File.Exists(GetProxyFilePath("MyCompany/Orders/Application/IOrderAppService.cs")).ShouldBeTrue();
        File.Exists(GetProxyFilePath("MyCompany/Orders/Application/OrderClientProxy.Generated.cs")).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Generate_Same_Named_Types_Of_Different_Namespaces_Into_Separate_Files_In_A_Single_Folder()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetOrderStateAsync", "MyCompany.Orders.StateDto"),
                CreateMethod("GetSharedStateAsync", "MyCompany.Shared.StateDto")));

        await _generator.GenerateProxyAsync(CreateArgs(folder: "Proxies"));

        (await File.ReadAllTextAsync(Path.Combine(_workDirectory, "Proxies", "MyCompany.Orders.StateDto.cs"))).ShouldContain("namespace MyCompany.Orders;");
        (await File.ReadAllTextAsync(Path.Combine(_workDirectory, "Proxies", "MyCompany.Shared.StateDto.cs"))).ShouldContain("namespace MyCompany.Shared;");
        File.Exists(Path.Combine(_workDirectory, "Proxies", "StateDto.cs")).ShouldBeFalse();

        var serviceInterface = await File.ReadAllTextAsync(Path.Combine(_workDirectory, "Proxies", "IOrderAppService.cs"));
        serviceInterface.ShouldContain("Task<global::MyCompany.Orders.StateDto> GetOrderStateAsync()");
        serviceInterface.ShouldContain("Task<global::MyCompany.Shared.StateDto> GetSharedStateAsync()");
    }

    [Fact]
    public async Task Should_Use_Full_Names_For_Same_Named_Array_Types_In_Service_Signatures()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetOrderStatesAsync", "MyCompany.Orders.StateDto[]"),
                CreateMethod("GetSharedStatesAsync", "MyCompany.Shared.StateDto[]")));

        await _generator.GenerateProxyAsync(CreateArgs());

        var serviceInterface = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/Application/IOrderAppService.cs"));
        serviceInterface.ShouldContain("Task<global::MyCompany.Orders.StateDto[]> GetOrderStatesAsync()");
        serviceInterface.ShouldContain("Task<global::MyCompany.Shared.StateDto[]> GetSharedStatesAsync()");

        var clientProxy = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/Application/OrderClientProxy.Generated.cs"));
        clientProxy.ShouldContain("public virtual async Task<global::MyCompany.Orders.StateDto[]> GetOrderStatesAsync()");
        clientProxy.ShouldContain("public virtual async Task<global::MyCompany.Shared.StateDto[]> GetSharedStatesAsync()");
        clientProxy.ShouldContain("return await RequestAsync<global::MyCompany.Orders.StateDto[]>(nameof(GetOrderStatesAsync));");
        clientProxy.ShouldContain("return await RequestAsync<global::MyCompany.Shared.StateDto[]>(nameof(GetSharedStatesAsync));");
    }

    [Fact]
    public async Task Should_Use_Valid_CSharp_Names_For_Nested_Clr_Types()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetNestedAsync", "MyCompany.Orders.Outer+InnerDto")));

        await _generator.GenerateProxyAsync(CreateArgs());

        var nestedDtoFile = GetProxyFilePath("MyCompany/Orders/Outer_Nested_InnerDto.cs");
        File.Exists(nestedDtoFile).ShouldBeTrue();
        (await File.ReadAllTextAsync(nestedDtoFile)).ShouldContain("public class Outer_Nested_InnerDto");

        var serviceInterface = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/Application/IOrderAppService.cs"));
        serviceInterface.ShouldContain("Task<Outer_Nested_InnerDto> GetNestedAsync()");
        serviceInterface.ShouldNotContain("+");

        var clientProxy = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/Application/OrderClientProxy.Generated.cs"));
        clientProxy.ShouldContain("public virtual async Task<Outer_Nested_InnerDto> GetNestedAsync()");
        clientProxy.ShouldNotContain("+");
    }

    [Fact]
    public async Task Should_Use_Existing_Nested_Type_Names_When_Contracts_Are_Generated()
    {
        WriteProjectAssets((
            ".NETCoreApp,Version=v10.0",
            DefaultClientAssemblyNames.Append("NestedContracts").ToArray()));
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetNestedAsync", "MyCompany.Orders.Outer+InnerDto")));
        _generator.Model.Types["MyCompany.Orders.Outer+InnerDto"].AssemblyName = "NestedContracts";

        await _generator.GenerateProxyAsync(CreateArgs());

        File.Exists(GetProxyFilePath("MyCompany/Orders/Outer_Nested_InnerDto.cs")).ShouldBeFalse();
        var serviceInterface = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/Application/IOrderAppService.cs"));
        serviceInterface.ShouldContain("Task<Outer.InnerDto> GetNestedAsync()");
        var clientProxy = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/Application/OrderClientProxy.Generated.cs"));
        clientProxy.ShouldContain("public virtual async Task<Outer.InnerDto> GetNestedAsync()");
        clientProxy.ShouldContain("return await RequestAsync<Outer.InnerDto>(nameof(GetNestedAsync));");
    }

    [Fact]
    public async Task Should_Use_Existing_Nested_Type_Names_Without_Contracts()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetNestedAsync", "MyCompany.Orders.Outer+InnerDto")));
        _generator.Model.Types.Clear();

        await _generator.GenerateProxyAsync(CreateArgs(withoutContracts: true));

        var clientProxy = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/Application/OrderClientProxy.Generated.cs"));
        clientProxy.ShouldContain("public virtual async Task<Outer.InnerDto> GetNestedAsync()");
        clientProxy.ShouldContain("return await RequestAsync<Outer.InnerDto>(nameof(GetNestedAsync));");
        clientProxy.ShouldNotContain("+");
        clientProxy.ShouldNotContain("_Nested_");
    }

    [Fact]
    public async Task Should_Generate_Same_Named_Generic_Types_Of_Different_Namespaces_Into_Separate_Files_In_A_Single_Folder()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetGenericBoxAsync", "MyCompany.Generic.Box<MyCompany.Shared.TagDto>"),
                CreateMethod("GetSharedBoxAsync", "MyCompany.Shared.Box<MyCompany.Shared.TagDto>")));
        AddGenericType(_generator.Model, "MyCompany.Shared.Box<T0>", new[] { "T" },
            ("Items", "[T]"));

        await _generator.GenerateProxyAsync(CreateArgs(folder: "Proxies"));

        var folder = Path.Combine(_workDirectory, "Proxies");
        var genericBox = await File.ReadAllTextAsync(Path.Combine(folder, "MyCompany.Generic.Box{T}.cs"));
        genericBox.ShouldContain("namespace MyCompany.Generic;");
        genericBox.ShouldContain("public class Box<T>");
        genericBox.ShouldContain("public T Content { get; set; }");

        var sharedBox = await File.ReadAllTextAsync(Path.Combine(folder, "MyCompany.Shared.Box{T}.cs"));
        sharedBox.ShouldContain("namespace MyCompany.Shared;");
        sharedBox.ShouldContain("public class Box<T>");
        sharedBox.ShouldContain("public T[] Items { get; set; }");

        File.Exists(Path.Combine(folder, "Box.cs")).ShouldBeFalse();
        File.Exists(Path.Combine(folder, "Box{T}.cs")).ShouldBeFalse();

        var serviceInterface = await File.ReadAllTextAsync(Path.Combine(folder, "IOrderAppService.cs"));
        serviceInterface.ShouldContain("Task<global::MyCompany.Generic.Box<TagDto>> GetGenericBoxAsync()");
        serviceInterface.ShouldContain("Task<global::MyCompany.Shared.Box<TagDto>> GetSharedBoxAsync()");

        var clientProxy = await File.ReadAllTextAsync(Path.Combine(folder, "OrderClientProxy.Generated.cs"));
        clientProxy.ShouldContain("return await RequestAsync<global::MyCompany.Generic.Box<TagDto>>(nameof(GetGenericBoxAsync));");
        clientProxy.ShouldContain("return await RequestAsync<global::MyCompany.Shared.Box<TagDto>>(nameof(GetSharedBoxAsync));");
    }

    [Fact]
    public async Task Should_Use_Full_Names_For_Same_Named_Types_Of_Different_Namespaces()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetStatesAsync", "MyCompany.Orders.StatesDto")));

        await _generator.GenerateProxyAsync(CreateArgs());

        var states = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/StatesDto.cs"));
        states.ShouldContain("public class StatesDto");
        states.ShouldContain("public global::MyCompany.Orders.StateDto OrderState { get; set; }");
        states.ShouldContain("public global::MyCompany.Shared.StateDto[] SharedStates { get; set; }");
        states.ShouldContain("public Wrapper<global::MyCompany.Shared.StateDto> Wrapped { get; set; }");
        (await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/StateDto.cs"))).ShouldContain("public class StateDto");
    }

    [Fact]
    public async Task Should_Ignore_Unreferenced_Types_When_Detecting_Ambiguous_Names()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetStateAsync", "MyCompany.Orders.StateDto")));
        AddType(_generator.Model, "MyCompany.Unrelated.StateDto", null);

        await _generator.GenerateProxyAsync(CreateArgs());

        var serviceInterface = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/Application/IOrderAppService.cs"));
        serviceInterface.ShouldContain("Task<StateDto> GetStateAsync()");
        serviceInterface.ShouldNotContain("global::MyCompany.Orders.StateDto");
    }

    [Fact]
    public async Task Should_Generate_Nested_Dictionaries()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetTagMapAsync", "MyCompany.Orders.TagMapDto")));

        await _generator.GenerateProxyAsync(CreateArgs());

        (await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/TagMapDto.cs")))
            .ShouldContain("public Dictionary<string, Dictionary<string, TagDto>> Tags { get; set; }");
        File.Exists(GetProxyFilePath("MyCompany/Shared/TagDto.cs")).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Use_Full_Names_For_Same_Named_Types_Without_Contracts()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetOrderStateAsync", "MyCompany.Orders.StateDto"),
                CreateMethod("GetSharedStateAsync", "MyCompany.Shared.StateDto")));
        _generator.Model.Types.Clear();

        await _generator.GenerateProxyAsync(CreateArgs(withoutContracts: true));

        var clientProxy = await File.ReadAllTextAsync(GetProxyFilePath("MyCompany/Orders/Application/OrderClientProxy.Generated.cs"));
        clientProxy.ShouldContain("using MyCompany.Orders;");
        clientProxy.ShouldContain("using MyCompany.Shared;");
        clientProxy.ShouldContain("public virtual async Task<global::MyCompany.Orders.StateDto> GetOrderStateAsync()");
        clientProxy.ShouldContain("public virtual async Task<global::MyCompany.Shared.StateDto> GetSharedStateAsync()");
    }

    private GenerateProxyArgs CreateArgs(string module = "orders", bool withoutContracts = false, string folder = null)
    {
        return new GenerateProxyArgs("generate-proxy", _workDirectory, module, null, null, null, null, null, folder, null, null, withoutContracts);
    }

    private string GetProxyFilePath(string relativePath)
    {
        return Path.Combine(_workDirectory, "ClientProxies", relativePath.Replace('/', Path.DirectorySeparatorChar));
    }

    private static ApplicationApiDescriptionModel CreateModel(params ControllerApiDescriptionModel[] controllers)
    {
        return CreateModuleModel("orders", controllers);
    }

    private static ApplicationApiDescriptionModel CreateModuleModel(string moduleName, params ControllerApiDescriptionModel[] controllers)
    {
        var module = ModuleApiDescriptionModel.Create(moduleName, moduleName);
        foreach (var controller in controllers)
        {
            module.Controllers[controller.Type] = controller;
        }

        var model = ApplicationApiDescriptionModel.Create();
        model.AddModule(module);

        AddType(model, "MyCompany.Orders.OrderDto", "Volo.Abp.Application.Dtos.ExtensibleEntityDto<System.Guid>",
            ("Customer", "MyCompany.Shared.CustomerDto"),
            ("Status", "MyCompany.Shared.OrderStatus?"),
            ("Lines", "[MyCompany.Orders.OrderLineDto]"));
        AddType(model, "MyCompany.Orders.OrderDetailDto", "MyCompany.Orders.OrderDto",
            ("Tags", "MyCompany.Shared.Wrapper<MyCompany.Shared.TagDto>"));
        AddType(model, "MyCompany.Orders.OrderLineDto", null,
            ("Price", "System.Decimal"));
        AddType(model, "MyCompany.Orders.GetOrdersInput", "Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto");
        AddType(model, "MyCompany.Orders.Application.UnusedDto", null);
        AddType(model, "MyCompany.Shared.CustomerDto", null,
            ("Addresses", "{System.String:MyCompany.Shared.AddressDto}"),
            ("ValueType", "Volo.Abp.Validation.StringValues.IStringValueType"));
        AddType(model, "MyCompany.Shared.AddressDto", null);
        AddType(model, "MyCompany.Shared.TagDto", null);
        AddGenericType(model, "MyCompany.Shared.Wrapper<T0>", new[] { "T" },
            ("Value", "T"),
            ("Items", "[T]"));
        AddType(model, "MyCompany.Shared.Result", null);
        AddGenericType(model, "MyCompany.Shared.Result<T0>", new[] { "T" },
            ("Value", "T"));
        AddGenericType(model, "MyCompany.Shared.Result<T0,T1>", new[] { "TItem", "TError" },
            ("Item", "TItem"),
            ("Error", "TError"));
        AddType(model, "Volo.Abp.ObjectExtending.OrdersModuleExtensionDto", null);
        AddType(model, "MyCompany.Orders.OrderOwnerDto", null,
            ("Owner", "Volo.Abp.Identity.IdentityUserDto"));
        AddType(model, "Volo.Abp.Identity.IdentityUserDto", "Volo.Abp.Application.Dtos.ExtensibleFullAuditedEntityDto<System.Guid>",
            ("UserName", "System.String"));
        AddType(model, "Volo.Abp.Users.UserData", null,
            ("UserName", "System.String"));
        AddType(model, "Volo.Abp.Identity.UserLookupSearchInputDto", null,
            ("Filter", "System.String"));
        AddType(model, "Volo.Abp.MultiTenancy.TenantConfiguration", null,
            ("Name", "System.String"));
        AddGenericType(model, "MyCompany.Generic.Box<T0>", new[] { "T" },
            ("Content", "T"));
        AddType(model, "MyCompany.Orders.BoxedOrderDto", null,
            ("Nested", "MyCompany.Generic.Box<MyCompany.Generic.Box<MyCompany.Shared.TagDto>>"));
        AddType(model, "MyCompany.Orders.StateDto", null);
        AddType(model, "MyCompany.Orders.Outer+InnerDto", null,
            ("NestedValue", "System.String"));
        AddType(model, "MyCompany.Orders.TagMapDto", null,
            ("Tags", "{System.String:{System.String:MyCompany.Shared.TagDto}}"));
        AddType(model, "MyCompany.Orders.StatesDto", null,
            ("OrderState", "MyCompany.Orders.StateDto"),
            ("SharedStates", "[MyCompany.Shared.StateDto]"),
            ("Wrapped", "MyCompany.Shared.Wrapper<MyCompany.Shared.StateDto>"));
        AddType(model, "MyCompany.Shared.StateDto", null);
        model.Types["MyCompany.Shared.OrderStatus"] = new TypeApiDescriptionModel
        {
            IsEnum = true,
            EnumNames = new[] { "Pending", "Shipped" },
            EnumValues = new object[] { 0, 1 }
        };

        AddType(model, "Volo.Abp.Application.Dtos.ExtensibleEntityDto<T0>", "Volo.Abp.ObjectExtending.ExtensibleObject");
        AddType(model, "Volo.Abp.Application.Dtos.PagedResultDto<T0>", "Volo.Abp.Application.Dtos.ListResultDto<T0>");
        AddType(model, "Volo.Abp.Application.Dtos.ListResultDto<T0>", null);
        AddType(model, "Volo.Abp.Application.Dtos.EntityDto", null);
        AddType(model, "Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto", null);
        AddType(model, "Volo.Abp.ObjectExtending.ExtensibleObject", null);
        AddType(model, "Volo.Abp.NameValue", null);
        AddType(model, "Volo.Abp.Content.IRemoteStreamContent", null);
        AddType(model, "Volo.Abp.Http.Modeling.ApplicationApiDescriptionModel", null);
        AddType(model, "Microsoft.AspNetCore.Http.IFormFile", null,
            ("Headers", "Microsoft.AspNetCore.Http.IHeaderDictionary"));
        AddType(model, "Volo.Abp.Validation.StringValues.IStringValueType", null,
            ("Validator", "Volo.Abp.Validation.StringValues.IValueValidator"));
        AddType(model, "Volo.Abp.Validation.StringValues.IValueValidator", null);

        model.Types["Volo.Abp.Identity.IdentityUserDto"].AssemblyName = "Volo.Abp.Identity.Application.Contracts";
        model.Types["Volo.Abp.Users.UserData"].AssemblyName = "Volo.Abp.Users.Abstractions";
        model.Types["Volo.Abp.Identity.UserLookupSearchInputDto"].AssemblyName = "Volo.Abp.Identity.Application.Contracts";
        model.Types["Volo.Abp.MultiTenancy.TenantConfiguration"].AssemblyName = "Volo.Abp.MultiTenancy.Abstractions";
        model.Types["Volo.Abp.ObjectExtending.OrdersModuleExtensionDto"].AssemblyName = "Volo.Abp.ObjectExtending";
        model.Types["Volo.Abp.Application.Dtos.ExtensibleEntityDto<T0>"].AssemblyName = "Volo.Abp.Ddd.Application.Contracts";
        model.Types["Volo.Abp.Application.Dtos.PagedResultDto<T0>"].AssemblyName = "Volo.Abp.Ddd.Application.Contracts";
        model.Types["Volo.Abp.Application.Dtos.ListResultDto<T0>"].AssemblyName = "Volo.Abp.Ddd.Application.Contracts";
        model.Types["Volo.Abp.Application.Dtos.EntityDto"].AssemblyName = "Volo.Abp.Ddd.Application.Contracts";
        model.Types["Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto"].AssemblyName = "Volo.Abp.Ddd.Application.Contracts";
        model.Types["Volo.Abp.ObjectExtending.ExtensibleObject"].AssemblyName = "Volo.Abp.ObjectExtending";
        model.Types["Volo.Abp.NameValue"].AssemblyName = "Volo.Abp.Core";
        model.Types["Volo.Abp.Content.IRemoteStreamContent"].AssemblyName = "Volo.Abp.Http.Abstractions";
        model.Types["Volo.Abp.Http.Modeling.ApplicationApiDescriptionModel"].AssemblyName = "Volo.Abp.Http";
        model.Types["Volo.Abp.Validation.StringValues.IStringValueType"].AssemblyName = "Volo.Abp.Validation.Abstractions";
        model.Types["Volo.Abp.Validation.StringValues.IValueValidator"].AssemblyName = "Volo.Abp.Validation.Abstractions";

        return model;
    }

    private static void AddType(ApplicationApiDescriptionModel model, string name, string baseType, params (string Name, string Type)[] properties)
    {
        model.Types[name] = new TypeApiDescriptionModel
        {
            BaseType = baseType,
            Properties = properties.Select(x => new PropertyApiDescriptionModel { Name = x.Name, Type = x.Type }).ToArray()
        };
    }

    private static void AddGenericType(ApplicationApiDescriptionModel model, string name, string[] genericArguments, params (string Name, string Type)[] properties)
    {
        AddType(model, name, null, properties);
        model.Types[name].GenericArguments = genericArguments;
    }

    private void WriteProjectAssets(params (string Target, string[] Assemblies)[] targets)
    {
        WriteProjectAssets(Path.Combine(_workDirectory, "obj", "project.assets.json"), targets);
    }

    private static void WriteProjectAssets(string assetsFilePath, params (string Target, string[] Assemblies)[] targets)
    {
        var targetData = targets.ToDictionary(
            x => x.Target,
            x => new Dictionary<string, object>
            {
                ["ClientDependencies/1.0.0"] = new
                {
                    compile = x.Assemblies.ToDictionary(
                        assemblyName => $"lib/net10.0/{assemblyName}.dll",
                        _ => new { })
                }
            });
        Directory.CreateDirectory(Path.GetDirectoryName(assetsFilePath)!);
        File.WriteAllText(assetsFilePath, JsonSerializer.Serialize(new { targets = targetData }));
    }

    private static ControllerApiDescriptionModel CreateServiceController(string serviceInterface, params InterfaceMethodApiDescriptionModel[] methods)
    {
        var serviceName = serviceInterface.Split('.').Last();
        return new ControllerApiDescriptionModel
        {
            ControllerName = serviceName.Substring(1).Replace("AppService", string.Empty),
            Type = serviceInterface.Replace($".{serviceName}", $".{serviceName.Substring(1)}"),
            Interfaces = new List<ControllerInterfaceApiDescriptionModel>
            {
                new() { Type = serviceInterface, Name = serviceName, Methods = methods }
            },
            Actions = methods.ToDictionary(x => x.Name, x => new ActionApiDescriptionModel
            {
                Name = x.Name,
                ImplementFrom = serviceInterface,
                ParametersOnMethod = x.ParametersOnMethod,
                Parameters = new List<ParameterApiDescriptionModel>(),
                ReturnValue = x.ReturnValue
            })
        };
    }

    private static InterfaceMethodApiDescriptionModel CreateMethod(string name, string returnType, params string[] parameterTypes)
    {
        return new InterfaceMethodApiDescriptionModel
        {
            Name = name,
            ParametersOnMethod = parameterTypes.Select((x, i) => new MethodParameterApiDescriptionModel { Name = $"p{i}", Type = x }).ToList(),
            ReturnValue = new ReturnValueApiDescriptionModel { Type = returnType }
        };
    }

    private class TestCSharpServiceProxyGenerator : CSharpServiceProxyGenerator
    {
        public ApplicationApiDescriptionModel Model { get; set; }

        public string WorkDirectory { get; set; }

        public string ProjectAssetsFilePath { get; set; }

        public int ProjectAssetsFilePathCallCount { get; private set; }

        public TestCSharpServiceProxyGenerator()
            : base(null, new AbpSystemTextJsonSerializer(Microsoft.Extensions.Options.Options.Create(new AbpSystemTextJsonSerializerOptions())))
        {
        }

        public List<string> GetTypes(ApplicationApiDescriptionModel model)
        {
            Model = model;
            GenerateProxyAsync(new GenerateProxyArgs(
                "generate-proxy",
                WorkDirectory,
                model.Modules.Keys.First(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                false)).GetAwaiter().GetResult();
            return TypesToGenerate;
        }

        protected override List<string> GetTypesToGenerate(ApplicationApiDescriptionModel applicationApiDescriptionModel)
        {
            TypesToGenerate = base.GetTypesToGenerate(applicationApiDescriptionModel);
            return TypesToGenerate;
        }

        protected override string GetProjectAssetsFilePath(string workDirectory)
        {
            ProjectAssetsFilePathCallCount++;
            return ProjectAssetsFilePath ?? base.GetProjectAssetsFilePath(workDirectory);
        }

        protected override Task<ApplicationApiDescriptionModel> GetApplicationApiDescriptionModelAsync(GenerateProxyArgs args, ApplicationApiDescriptionModelRequestDto requestDto = null)
        {
            return Task.FromResult(Model);
        }

        private List<string> TypesToGenerate { get; set; }
    }
}
