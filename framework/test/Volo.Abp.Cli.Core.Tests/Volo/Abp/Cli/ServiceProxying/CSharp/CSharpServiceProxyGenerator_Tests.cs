using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json.SystemTextJson;
using Xunit;

namespace Volo.Abp.Cli.ServiceProxying.CSharp;

public class CSharpServiceProxyGenerator_Tests : IDisposable
{
    private const string OrderAppService = "MyCompany.Orders.Application.IOrderAppService";

    private readonly TestCSharpServiceProxyGenerator _generator = new();
    private readonly string _workDirectory;

    public CSharpServiceProxyGenerator_Tests()
    {
        _workDirectory = Path.Combine(Path.GetTempPath(), "abp-csharp-proxy-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_workDirectory);
        File.WriteAllText(Path.Combine(_workDirectory, "Client.csproj"), "<Project Sdk=\"Microsoft.NET.Sdk\" />");
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
    public void Should_Not_Generate_Types_Of_Abp_Modules()
    {
        var model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetOwnerAsync", "MyCompany.Orders.OrderOwnerDto"),
                CreateMethod("GetUserAsync", "Volo.Abp.Users.UserData"),
                CreateMethod("GetExtensionAsync", "Volo.Abp.ObjectExtending.OrdersModuleExtensionDto")));

        _generator.GetTypes(model).ShouldBe(new[] { "MyCompany.Orders.OrderOwnerDto" });
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
    public async Task Should_Remove_Stale_Generated_Files_When_Regenerating()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetAsync", "MyCompany.Orders.OrderDto"),
                CreateMethod("GetStatusesAsync", "[MyCompany.Shared.OrderStatus]")));
        await _generator.GenerateProxyAsync(CreateArgs());
        File.Exists(GetProxyFilePath("MyCompany/Orders/OrderDto.cs")).ShouldBeTrue();
        File.Exists(GetProxyFilePath("MyCompany/Shared/CustomerDto.cs")).ShouldBeTrue();

        var otherModuleDto = GetProxyFilePath("MyCompany/Shared/AddressDto.cs");
        var userFile = GetProxyFilePath("MyCompany/Orders/OrderDtoExtensions.cs");
        await File.WriteAllTextAsync(userFile, "namespace MyCompany.Orders;");
        await GenerateOtherModuleAsync();

        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetStatusesAsync", "[MyCompany.Shared.OrderStatus]")));
        await _generator.GenerateProxyAsync(CreateArgs());

        File.Exists(GetProxyFilePath("MyCompany/Orders/OrderDto.cs")).ShouldBeFalse();
        File.Exists(GetProxyFilePath("MyCompany/Orders/OrderLineDto.cs")).ShouldBeFalse();
        File.Exists(GetProxyFilePath("MyCompany/Shared/CustomerDto.cs")).ShouldBeFalse();
        File.Exists(GetProxyFilePath("MyCompany/Shared/OrderStatus.cs")).ShouldBeTrue();
        File.Exists(GetProxyFilePath("MyCompany/Orders/Application/IOrderAppService.cs")).ShouldBeTrue();
        File.Exists(GetProxyFilePath("MyCompany/Orders/Application/OrderClientProxy.Generated.cs")).ShouldBeTrue();
        File.Exists(GetProxyFilePath("MyCompany/Orders/Application/OrderClientProxy.cs")).ShouldBeTrue();
        File.Exists(otherModuleDto).ShouldBeTrue();
        File.Exists(userFile).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Remove_Stale_Contracts_When_Regenerating_Without_Contracts()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetStatusesAsync", "[MyCompany.Shared.OrderStatus]")));
        await _generator.GenerateProxyAsync(CreateArgs());
        File.Exists(GetProxyFilePath("MyCompany/Shared/OrderStatus.cs")).ShouldBeTrue();
        File.Exists(GetProxyFilePath("MyCompany/Orders/Application/IOrderAppService.cs")).ShouldBeTrue();

        _generator.Model.Types.Clear();
        await _generator.GenerateProxyAsync(CreateArgs(withoutContracts: true));

        File.Exists(GetProxyFilePath("MyCompany/Shared/OrderStatus.cs")).ShouldBeFalse();
        File.Exists(GetProxyFilePath("MyCompany/Orders/Application/IOrderAppService.cs")).ShouldBeFalse();
        File.Exists(GetProxyFilePath("MyCompany/Orders/Application/OrderClientProxy.Generated.cs")).ShouldBeTrue();
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
    public async Task Should_Not_Remove_Files_Of_Proxies_Generated_Into_A_Sub_Folder()
    {
        _generator.Model = CreateModuleModel("customers",
            CreateServiceController("MyCompany.Customers.Application.IAddressAppService",
                CreateMethod("GetAsync", "MyCompany.Shared.AddressDto")));
        await _generator.GenerateProxyAsync(CreateArgs(module: "customers", folder: "Proxies/Customers"));

        var customerFiles = Directory.GetFiles(Path.Combine(_workDirectory, "Proxies", "Customers"), "*.cs");
        customerFiles.ShouldContain(x => x.EndsWith("AddressDto.cs"));

        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetStatusesAsync", "[MyCompany.Shared.OrderStatus]")));
        await _generator.GenerateProxyAsync(CreateArgs(folder: "Proxies"));

        foreach (var customerFile in customerFiles)
        {
            File.Exists(customerFile).ShouldBeTrue();
        }
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
    public async Task Should_Keep_Regenerated_Files_Whose_Existing_Name_Has_A_Different_Casing()
    {
        _generator.Model = CreateModel(
            CreateServiceController(OrderAppService,
                CreateMethod("GetStatusesAsync", "[MyCompany.Shared.OrderStatus]")));
        await _generator.GenerateProxyAsync(CreateArgs());

        var folder = Path.GetDirectoryName(GetProxyFilePath("MyCompany/Shared/OrderStatus.cs"))!;
        File.Move(Path.Combine(folder, "OrderStatus.cs"), Path.Combine(folder, "ORDERSTATUS.cs"));

        await _generator.GenerateProxyAsync(CreateArgs());

        var orderStatusFile = Directory.GetFiles(folder).SingleOrDefault(x => Path.GetFileName(x).Equals("OrderStatus.cs", StringComparison.OrdinalIgnoreCase));
        orderStatusFile.ShouldNotBeNull();
        (await File.ReadAllTextAsync(orderStatusFile)).ShouldContain("public enum OrderStatus");
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

    private async Task GenerateOtherModuleAsync()
    {
        var model = _generator.Model;
        _generator.Model = CreateModuleModel("customers",
            CreateServiceController("MyCompany.Customers.Application.IAddressAppService",
                CreateMethod("GetAsync", "MyCompany.Shared.AddressDto")));
        await _generator.GenerateProxyAsync(CreateArgs(module: "customers"));
        _generator.Model = model;
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
        AddGenericType(model, "MyCompany.Generic.Box<T0>", new[] { "T" },
            ("Content", "T"));
        AddType(model, "MyCompany.Orders.BoxedOrderDto", null,
            ("Nested", "MyCompany.Generic.Box<MyCompany.Generic.Box<MyCompany.Shared.TagDto>>"));
        AddType(model, "MyCompany.Orders.StateDto", null);
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

        public TestCSharpServiceProxyGenerator()
            : base(null, new AbpSystemTextJsonSerializer(Microsoft.Extensions.Options.Options.Create(new AbpSystemTextJsonSerializerOptions())))
        {
        }

        public List<string> GetTypes(ApplicationApiDescriptionModel model)
        {
            return GetTypesToGenerate(model);
        }

        protected override Task<ApplicationApiDescriptionModel> GetApplicationApiDescriptionModelAsync(GenerateProxyArgs args, ApplicationApiDescriptionModelRequestDto requestDto = null)
        {
            return Task.FromResult(Model);
        }
    }
}
