using System.Collections.Generic;

namespace Volo.Abp.Studio.Packages
{
    public static class PackageTypes
    {
        public const string Domain = "lib.domain";
        public const string DomainShared = "lib.domain-shared";
        public const string Application = "lib.application";
        public const string ApplicationContracts = "lib.application-contracts";
        public const string EntityFrameworkCore = "lib.ef";
        public const string MongoDB = "lib.mongodb";
        public const string HttpApi = "lib.http-api";
        public const string HttpApiClient = "lib.http-api-client";
        public const string Mvc = "lib.mvc";
        public const string Blazor = "lib.blazor";
        public const string BlazorWebAssembly = "lib.blazor-wasm";
        public const string BlazorServer = "lib.blazor-server";
        public const string Test = "lib.test";

        public const string HostHttpApi = "host.http-api";
        public const string HostMvc = "host.mvc";
        public const string HostBlazorWebAssembly = "host.blazor-wasm";
        public const string HostBlazorServer = "host.blazor-server";
        public const string HostApiGatewayOcelot = "host.api-gateway-ocelot";

        public static string CalculateDefaultPackageNameForModule(
            string moduleName,
            string packageType)
        {
            switch (packageType)
            {
                case Domain:
                    return moduleName + ".Domain";
                case DomainShared:
                    return moduleName + ".Domain.Shared";
                case Application:
                    return moduleName + ".Application";
                case ApplicationContracts:
                    return moduleName + ".Application.Contracts";
                case EntityFrameworkCore:
                    return moduleName + ".EntityFrameworkCore";
                case HttpApi:
                    return moduleName + ".HttpApi";
                case HttpApiClient:
                    return moduleName + ".HttpApi.Client";
                case MongoDB:
                    return moduleName + ".MongoDB";
                case Mvc:
                    return moduleName + ".Web";
                case Blazor:
                    return moduleName + ".Blazor";
                case BlazorWebAssembly:
                    return moduleName + ".Blazor.WebAssembly";
                case BlazorServer:
                    return moduleName + ".Blazor.Server";
                case HostHttpApi:
                    return moduleName + ".HttpApi.Host";
                case HostMvc:
                    return moduleName + ".Web.Host";
                case HostBlazorWebAssembly:
                    return moduleName + ".Blazor.Client";
                case HostBlazorServer:
                    return moduleName + ".Blazor.Host";
                case HostApiGatewayOcelot:
                    return moduleName + ".Gateway";
                default:
                    throw new AbpStudioException(AbpStudioErrorCodes.PackageNameMustBeSpecified);
            }
        }

        public static bool IsHostProject(string packageType)
        {
            return
                packageType == HostMvc ||
                packageType == HostHttpApi ||
                packageType == HostBlazorWebAssembly ||
                packageType == HostBlazorServer ||
                packageType == HostApiGatewayOcelot;
        }

        public static bool IsUiProject(string packageType)
        {
            return
                packageType == Mvc ||
                packageType == BlazorWebAssembly ||
                packageType == BlazorServer;
        }

        public static string GetHostTypeOfUi(string packageType, bool useHostBlazorServerForMvcPackages = false)
        {
            return packageType switch
            {
                Mvc => !useHostBlazorServerForMvcPackages ? HostMvc : HostBlazorServer,
                BlazorWebAssembly => HostBlazorWebAssembly,
                BlazorServer => HostBlazorServer,
                _ => null
            };
        }

        // TODO: Rename this method, or (maybe and) write a summary. Also consider other suggestions
        public static List<string> GetDddTargetProjectRole(string packageType)
        {
            if (packageType == DomainShared)
            {
                return new List<string>()
                {
                    Domain,
                    ApplicationContracts
                };
            }

            if (packageType == Domain)
            {
                return new List<string>()
                {
                    Application
                };
            }

            if (packageType == ApplicationContracts)
            {
                return new List<string>()
                {
                    Application,
                    Mvc,
                    Blazor,
                    BlazorServer,
                    BlazorWebAssembly,
                    HttpApi
                };
            }

            if (packageType == Application)
            {
                return new List<string>()
                {
                    Application,
                    HostMvc,
                    HostBlazorServer,
                    HostBlazorWebAssembly,
                    HostHttpApi
                };
            }

            if (packageType == EntityFrameworkCore)
            {
                return new List<string>()
                {
                    HostMvc,
                    HostBlazorServer,
                    HostBlazorWebAssembly,
                    HostHttpApi
                };
            }

            if (packageType == MongoDB)
            {
                return new List<string>()
                {
                    HostMvc,
                    HostBlazorServer,
                    HostBlazorWebAssembly,
                    HostHttpApi
                };
            }

            if (packageType == HttpApi)
            {
                return new List<string>()
                {
                    HostMvc,
                    HostBlazorServer,
                    HostBlazorWebAssembly,
                    HostHttpApi
                };
            }

            if (packageType == HttpApiClient)
            {
                return new List<string>()
                {
                    HostMvc,
                    HostBlazorServer,
                    HostBlazorWebAssembly,
                    HostHttpApi
                };
            }

            if (packageType == HttpApiClient)
            {
                return new List<string>()
                {
                    HostMvc,
                    HostBlazorServer,
                    HostBlazorWebAssembly,
                    HostHttpApi
                };
            }

            if (packageType == Mvc)
            {
                return new List<string>()
                {
                    HostMvc
                };
            }

            if (packageType == Blazor)
            {
                return new List<string>()
                {
                    BlazorServer,
                    BlazorWebAssembly
                };
            }

            if (packageType == BlazorServer)
            {
                return new List<string>()
                {
                    HostBlazorServer
                };
            }

            if (packageType == BlazorWebAssembly)
            {
                return new List<string>()
                {
                    HostBlazorWebAssembly
                };
            }

            if (packageType == Test)
            {
                return new List<string>()
                {
                    Test
                };
            }

            return new List<string>();
        }
    }
}
