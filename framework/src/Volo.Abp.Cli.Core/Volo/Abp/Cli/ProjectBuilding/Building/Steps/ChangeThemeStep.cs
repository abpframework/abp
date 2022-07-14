using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Volo.Abp.Cli.ProjectBuilding.Files;
using Volo.Abp.Cli.Utils;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

//TODO: refactor!!!
public class ChangeThemeStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        if (!context.BuildArgs.Theme.HasValue)
        {
            return;
        }

        switch (context.BuildArgs.Theme)
        {
            case Theme.Basic:
                ChangeToBasicTheme(context);
                break;
            case Theme.Lepton:
                ChangeToLeptonTheme(context);
                break;
        }
    }

    //TODO: make it more dynamic to support leptonx to basic theme transition!!!
    protected void ChangeToBasicTheme(ProjectBuildContext context)
    {
        #region MyCompanyName.MyProjectName.Web

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web/MyCompanyName.MyProjectName.Web.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );

        #endregion
        
        #region MyCompanyName.MyProjectName.HttpApi.Host

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/MyCompanyName.MyProjectName.HttpApi.Host.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/MyProjectNameHttpApiHostModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.Host/MyProjectNameHttpApiHostModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );

        #endregion
        
        #region MyCompanyName.MyProjectName.Blazor
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyCompanyName.MyProjectName.Blazor.csproj",
            "Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXLiteTheme",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme\Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.csproj"
            );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXLiteTheme",
            "Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme",
            "AbpAspNetCoreComponentsWebAssemblyLeptonXLiteThemeModule",
            "AbpAspNetCoreComponentsWebAssemblyBasicThemeModule"
        );

        ChangeNamespace(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite",
            "Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic"
        );
        
        #endregion

        #region MyCompanyName.MyProjectName.Blazor.Server
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyCompanyName.MyProjectName.Blazor.csproj",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
            );

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyCompanyName.MyProjectName.Blazor.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.Server.BasicTheme\Volo.Abp.AspNetCore.Components.Server.BasicTheme.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme",
            "AbpAspNetCoreComponentsServerLeptonXLiteThemeModule",
            "AbpAspNetCoreComponentsServerBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
            );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "BlazorLeptonXLiteThemeBundles.Styles.Global",
            "BlazorBasicThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite",
            "Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic",
            "BlazorLeptonXLiteThemeBundles.Styles.Global",
            "BlazorBasicThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
            "BlazorLeptonXLiteThemeBundles.Scripts.Global",
            "BlazorBasicThemeBundles.Scripts.Global"
        );
        
        #endregion
        
        //TODO: refactor!!! (same with blazor.server except paths)
        #region MyCompanyName.MyProjectName.Blazor.Server.Tiered

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyCompanyName.MyProjectName.Blazor.Server.Tiered.csproj",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyCompanyName.MyProjectName.Blazor.Server.Tiered.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.Server.BasicTheme\Volo.Abp.AspNetCore.Components.Server.BasicTheme.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme",
            "AbpAspNetCoreComponentsServerLeptonXLiteThemeModule",
            "AbpAspNetCoreComponentsServerBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "BlazorLeptonXLiteThemeBundles.Styles.Global",
            "BlazorBasicThemeBundles.Styles.Global"
        );
        
                
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite",
            "Volo.Abp.AspNetCore.Components.Web.BasicTheme.Themes.Basic",
            "BlazorLeptonXLiteThemeBundles.Styles.Global",
            "BlazorBasicThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
            "BlazorLeptonXLiteThemeBundles.Scripts.Global",
            "BlazorBasicThemeBundles.Scripts.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.AuthServer

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.AuthServer/MyCompanyName.MyProjectName.AuthServer.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.AuthServer/MyProjectNameAuthServerModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.AuthServer/MyProjectNameAuthServerModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.Web.Mvc
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc/MyCompanyName.MyProjectName.Web.Mvc.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
            );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.Web.Mvc.Mongo

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc.Mongo/MyCompanyName.MyProjectName.Web.Mvc.Mongo.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc.Mongo/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc.Mongo/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.Blazor.Server - (app-nolayers)
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme",
            "AbpAspNetCoreComponentsServerLeptonXLiteThemeModule",
            "AbpAspNetCoreComponentsServerBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "BlazorLeptonXLiteThemeBundles.Styles.Global",
            "BlazorBasicThemeBundles.Styles.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.Blazor.Server.Mongo

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyCompanyName.MyProjectName.Blazor.Server.Mongo.csproj",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.csproj"
        );
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyCompanyName.MyProjectName.Blazor.Server.Mongo.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            @"..\..\..\..\..\modules\basic-theme\src\Volo.Abp.AspNetCore.Components.Server.BasicTheme\Volo.Abp.AspNetCore.Components.Server.BasicTheme.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme",
            "AbpAspNetCoreComponentsServerLeptonXLiteThemeModule",
            "AbpAspNetCoreComponentsServerBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic",
            "AbpAspNetCoreMvcUiLeptonXLiteThemeModule",
            "AbpAspNetCoreMvcUiBasicThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.BasicTheme.Bundling",
            "LeptonXLiteThemeBundles.Styles.Global",
            "BasicThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling",
            "BlazorLeptonXLiteThemeBundles.Styles.Global",
            "BlazorBasicThemeBundles.Styles.Global"
        );
        
        #endregion

        #region Angular

        ReplaceImportPackage(
            context, 
            "/angular/src/app/app.module.ts",
            "@abp/ng.theme.lepton-x", 
            "@abp/ng.theme.basic"
        );

        RemoveLinesByStatement(
            context,
            "/angular/src/app/app.module.ts",
        "SideMenuLayoutModule"
        );

        ReplaceMethodNames(
            context,
            "/angular/src/app/app.module.ts",
            "ThemeLeptonXModule",
            "ThemeBasicModule"
        );
        
        RemoveLinesByStatement(
            context,
            "/angular/angular.json",
            "node_modules/bootstrap-icons/font/bootstrap-icons.css"
        );

        #endregion
    }
    
    protected void ChangeToLeptonTheme(ProjectBuildContext context)
    {
        #region Common 
        
        RenameLeptonXFolders(context, folderName: "Lepton");
        AddLeptonThemeManagementReferenceToProjects(context);
        
        #endregion

        #region MyCompanyName.MyProjectName.Web

        ChangeThemeToLeptonForMvc(context, "Web");

        #endregion

        #region MyCompanyName.MyProjectName.HttpApi.Host

        ChangeThemeToLeptonForMvc(context, "HttpApi.Host");

        #endregion
        
        #region MyCompanyName.MyProjectName.Blazor
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyCompanyName.MyProjectName.Blazor.csproj",
            "Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme",
            @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Components.WebAssembly.LeptonTheme\Volo.Abp.AspNetCore.Components.WebAssembly.LeptonTheme.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components",
            "Volo.Abp.AspNetCore.Components.Web.LeptonTheme.Components",
            "AbpAspNetCoreComponentsWebAssemblyLeptonXThemeModule",
            "AbpAspNetCoreComponentsWebAssemblyLeptonThemeModule"
        );

        #endregion

        #region MyCompanyName.MyProjectName.Blazor.Server

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyCompanyName.MyProjectName.Blazor.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.csproj"
        );

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyCompanyName.MyProjectName.Blazor.csproj",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
            @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Components.Server.LeptonTheme\Volo.Abp.AspNetCore.Components.Server.LeptonTheme.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
            "Volo.Abp.AspNetCore.Components.Server.LeptonTheme",
            "AbpAspNetCoreComponentsServerLeptonXThemeModule",
            "AbpAspNetCoreComponentsServerLeptonThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton",
            "AbpAspNetCoreMvcUiLeptonXThemeModule",
            "AbpAspNetCoreMvcUiLeptonThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling",
            "LeptonXThemeBundles.Styles.Global",
            "LeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling",
            "BlazorLeptonXThemeBundles.Styles.Global",
            "BlazorLeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components",
            "Volo.Abp.AspNetCore.Components.Web.LeptonTheme.Components",
            "BlazorLeptonXThemeBundles.Styles.Global",
            "BlazorLeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling",
            "BlazorLeptonXThemeBundles.Scripts.Global",
            "BlazorLeptonThemeBundles.Scripts.Global"
        );

        #endregion

        //TODO: refactor!!! (same with blazor.server except paths)
        #region MyCompanyName.MyProjectName.Blazor.Server.Tiered 

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyCompanyName.MyProjectName.Blazor.Server.Tiered.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.csproj"
        );

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyCompanyName.MyProjectName.Blazor.Server.Tiered.csproj",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
            @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Components.Server.LeptonTheme\Volo.Abp.AspNetCore.Components.Server.LeptonTheme.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
            "Volo.Abp.AspNetCore.Components.Server.LeptonTheme",
            "AbpAspNetCoreComponentsServerLeptonXThemeModule",
            "AbpAspNetCoreComponentsServerLeptonThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton",
            "AbpAspNetCoreMvcUiLeptonXThemeModule",
            "AbpAspNetCoreMvcUiLeptonThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling",
            "LeptonXThemeBundles.Styles.Global",
            "LeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/MyProjectNameBlazorModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling",
            "BlazorLeptonXThemeBundles.Styles.Global",
            "BlazorLeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components",
            "Volo.Abp.AspNetCore.Components.Web.LeptonTheme.Components",
            "BlazorLeptonXThemeBundles.Styles.Global",
            "BlazorLeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling",
            "BlazorLeptonXThemeBundles.Scripts.Global",
            "BlazorLeptonThemeBundles.Scripts.Global"
        );

        #endregion
        
        #region MyCompanyName.MyProjectName.AuthServer

        ChangeThemeToLeptonForMvc(context, "AuthServer");
        
        #endregion

        #region Angular

        ReplaceImportPackage(
            context, 
            "/angular/src/app/app.module.ts",
            "@volosoft/abp.ng.theme.lepton-x", 
            "@volo/abp.ng.theme.lepton"
        );

        RemoveLinesByStatement(
            context,
            "/angular/src/app/app.module.ts",
            "SideMenuLayoutModule"
        );

        ReplaceMethodNames(
            context,
            "/angular/src/app/app.module.ts",
            "ThemeLeptonXModule",
            "ThemeLeptonModule"
        );
        
        RemoveLinesByStatement(
            context,
            "/angular/angular.json",
            "node_modules/bootstrap-icons/font/bootstrap-icons.css"
        );

        #endregion

        //TODO: refactor!!!
        // Single-Layers
        
        #region MyCompanyName.MyProjectName.Web.Mvc
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc/MyCompanyName.MyProjectName.Web.Mvc.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            @"..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.csproj"
        );
        
        var file = context.Files.FirstOrDefault(f => !f.Name.Contains("Test") && f.Name.Contains(@"/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc/MyCompanyName.MyProjectName.Web.Mvc.csproj") && f.Name.Contains(".csproj"));
        if (file != null)
        {
            AddProjectReference(file, @"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.Web\Volo.Abp.LeptonTheme.Management.Web.csproj");
            AddModuleDependency(ConvertProjectFileToModuleFile(context, file), $"LeptonThemeManagement{ConvertProjectNameToModuleName("Web")}Module");
            
            AddProjectReference(file, @"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.HttpApi\Volo.Abp.LeptonTheme.Management.HttpApi.csproj");
            AddModuleDependency(ConvertProjectFileToModuleFile(context, file), $"LeptonThemeManagement{ConvertProjectNameToModuleName("HttpApi")}Module", underManagementFolder: false);

            AddProjectReference(file, @"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.Application\Volo.Abp.LeptonTheme.Management.Application.csproj");
            AddModuleDependency(ConvertProjectFileToModuleFile(context, file), $"LeptonThemeManagement{ConvertProjectNameToModuleName("Application")}Module");
        }
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton",
            "AbpAspNetCoreMvcUiLeptonXThemeModule",
            "AbpAspNetCoreMvcUiLeptonThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling",
            "LeptonXThemeBundles.Styles.Global",
            "LeptonThemeBundles.Styles.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.Web.Mvc.Mongo

        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc.Mongo/MyCompanyName.MyProjectName.Web.Mvc.Mongo.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            @"..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.csproj"
        );
        
        var file2 = context.Files.FirstOrDefault(f => !f.Name.Contains("Test") && f.Name.Contains(@"/aspnet-core/src/MMyCompanyName.MyProjectName.Web.Mvc.Mongo/MyCompanyName.MyProjectName.Web.Mvc.Mongo.csproj") && f.Name.Contains(".csproj"));
        if (file2 != null)
        {
            AddProjectReference(file2, @"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.Web\Volo.Abp.LeptonTheme.Management.Web.csproj");
            AddModuleDependency(ConvertProjectFileToModuleFile(context, file2), $"LeptonThemeManagement{ConvertProjectNameToModuleName("Web")}Module");
            
            AddProjectReference(file2, @"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.HttpApi\Volo.Abp.LeptonTheme.Management.HttpApi.csproj");
            AddModuleDependency(ConvertProjectFileToModuleFile(context, file2), $"LeptonThemeManagement{ConvertProjectNameToModuleName("HttpApi")}Module", underManagementFolder: false);

            AddProjectReference(file2, @"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.Application\Volo.Abp.LeptonTheme.Management.Application.csproj");
            AddModuleDependency(ConvertProjectFileToModuleFile(context, file2), $"LeptonThemeManagement{ConvertProjectNameToModuleName("Application")}Module");
        }
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc.Mongo/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton",
            "AbpAspNetCoreMvcUiLeptonXThemeModule",
            "AbpAspNetCoreMvcUiLeptonThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Mvc.Mongo/MyProjectNameWebModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling",
            "LeptonXThemeBundles.Styles.Global",
            "LeptonThemeBundles.Styles.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.Blazor.Server - (app-nolayers)
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyCompanyName.MyProjectName.Blazor.Server.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            @"..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.csproj"
        );
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyCompanyName.MyProjectName.Blazor.Server.csproj",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
            @"..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Components.Server.LeptonTheme\Volo.Abp.AspNetCore.Components.Server.LeptonTheme.csproj"
        );
        
        var file3 = context.Files.FirstOrDefault(f => !f.Name.Contains("Test") && f.Name.Contains(@"/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyCompanyName.MyProjectName.Blazor.Server.csproj") && f.Name.Contains(".csproj"));
        if (file3 != null)
        {
            AddProjectReference(file3, @"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.Blazor.Server\Volo.Abp.LeptonTheme.Management.Blazor.Server.csproj");
            AddModuleDependency(ConvertProjectFileToModuleFile(context, file3), $"LeptonThemeManagement{ConvertProjectNameToModuleName("Blazor.Server")}Module");
            
            AddProjectReference(file3, @"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.HttpApi\Volo.Abp.LeptonTheme.Management.HttpApi.csproj");
            AddModuleDependency(ConvertProjectFileToModuleFile(context, file3), $"LeptonThemeManagement{ConvertProjectNameToModuleName("HttpApi")}Module", underManagementFolder: false);

            AddProjectReference(file3, @"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.Application\Volo.Abp.LeptonTheme.Management.Application.csproj");
            AddModuleDependency(ConvertProjectFileToModuleFile(context, file3), $"LeptonThemeManagement{ConvertProjectNameToModuleName("Application")}Module");
        }

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
            "Volo.Abp.AspNetCore.Components.Server.LeptonTheme",
            "AbpAspNetCoreComponentsServerLeptonXThemeModule",
            "AbpAspNetCoreComponentsServerLeptonThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton",
            "AbpAspNetCoreMvcUiLeptonXThemeModule",
            "AbpAspNetCoreMvcUiLeptonThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling",
            "LeptonXThemeBundles.Styles.Global",
            "LeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling",
            "BlazorLeptonXThemeBundles.Styles.Global",
            "BlazorLeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components",
            "Volo.Abp.AspNetCore.Components.Web.LeptonTheme.Components",
            "BlazorLeptonXThemeBundles.Styles.Global",
            "BlazorLeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling",
            "BlazorLeptonXThemeBundles.Scripts.Global",
            "BlazorLeptonThemeBundles.Scripts.Global"
        );
        
        #endregion
        
        #region MyCompanyName.MyProjectName.Blazor.Server.Mongo 
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyCompanyName.MyProjectName.Blazor.Server.Mongo.csproj",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            @"..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.csproj"
        );
        
        ReplacePackageReferenceWithProjectReference(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyCompanyName.MyProjectName.Blazor.Server.Mongo.csproj",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
            @"..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Components.Server.LeptonTheme\Volo.Abp.AspNetCore.Components.Server.LeptonTheme.csproj"
        );
        
        var file4 = context.Files.FirstOrDefault(f => !f.Name.Contains("Test") && f.Name.Contains(@"/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyCompanyName.MyProjectName.Blazor.Server.Mongo.csproj") && f.Name.Contains(".csproj"));
        if (file4 != null)
        {
            AddProjectReference(file4, @"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.Blazor.Server\Volo.Abp.LeptonTheme.Management.Blazor.Server.csproj");
            AddModuleDependency(ConvertProjectFileToModuleFile(context, file4), $"LeptonThemeManagement{ConvertProjectNameToModuleName("Blazor.Server")}Module");
            
            AddProjectReference(file4, @"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.HttpApi\Volo.Abp.LeptonTheme.Management.HttpApi.csproj");
            AddModuleDependency(ConvertProjectFileToModuleFile(context, file4), $"LeptonThemeManagement{ConvertProjectNameToModuleName("HttpApi")}Module", underManagementFolder: false);

            AddProjectReference(file4, @"..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.Application\Volo.Abp.LeptonTheme.Management.Application.csproj");
            AddModuleDependency(ConvertProjectFileToModuleFile(context, file4), $"LeptonThemeManagement{ConvertProjectNameToModuleName("Application")}Module");
        }

        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme",
            "Volo.Abp.AspNetCore.Components.Server.LeptonTheme",
            "AbpAspNetCoreComponentsServerLeptonXThemeModule",
            "AbpAspNetCoreComponentsServerLeptonThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton",
            "AbpAspNetCoreMvcUiLeptonXThemeModule",
            "AbpAspNetCoreMvcUiLeptonThemeModule"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling",
            "LeptonXThemeBundles.Styles.Global",
            "LeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/MyProjectNameModule.cs",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling",
            "BlazorLeptonXThemeBundles.Styles.Global",
            "BlazorLeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components",
            "Volo.Abp.AspNetCore.Components.Web.LeptonTheme.Components",
            "BlazorLeptonXThemeBundles.Styles.Global",
            "BlazorLeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Mongo/Pages/_Host.cshtml",
            "Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Bundling",
            "Volo.Abp.AspNetCore.Components.Server.LeptonTheme.Bundling",
            "BlazorLeptonXThemeBundles.Scripts.Global",
            "BlazorLeptonThemeBundles.Scripts.Global"
        );
        
        #endregion
        
        // Microservices
    }

    private void ChangeThemeToLeptonForMvc(ProjectBuildContext context, string projectName)
    {
        var projectPath = $"/MyCompanyName.MyProjectName.{projectName}/MyCompanyName.MyProjectName.{projectName}.csproj";
        var projectFile = context.Files.FirstOrDefault(x => x.Name.Contains(projectPath));
        if (projectFile is null)
        {
            return;
        }
        
        var moduleFile = ConvertProjectFileToModuleFile(context, projectFile);
        
        ReplacePackageReferenceWithProjectReference(
            context,
            projectFile.Name,
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            @"..\..\..\..\..\lepton-theme\src\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton\Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.csproj"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            moduleFile.Name,
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling",
            "LeptonXThemeBundles.Styles.Global",
            "LeptonThemeBundles.Styles.Global"
        );
        
        ChangeNamespaceAndKeyword(
            context,
            moduleFile.Name,
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX",
            "Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton",
            "AbpAspNetCoreMvcUiLeptonXThemeModule",
            "AbpAspNetCoreMvcUiLeptonThemeModule"
        );
        
        RemoveLinesByStatement(
            context,
            moduleFile.Name,
            "using Volo.Abp.LeptonX.Shared;"
        );
    }
    
    private void AddLeptonThemeManagementReferenceToProjects(ProjectBuildContext context)
    {
        var projects = new Dictionary<string, string> 
        {
            {"Domain", "MyCompanyName.MyProjectName.Domain.csproj"},
            {"Domain.Shared", "MyCompanyName.MyProjectName.Domain.Shared.csproj"},
            {"Application", "MyCompanyName.MyProjectName.Application.csproj"},
            {"Application.Contracts", "MyCompanyName.MyProjectName.Application.Contracts.csproj"},
            {"Blazor.WebAssembly", "MyCompanyName.MyProjectName.Blazor.csproj"},
            {"Blazor.Server", "MyCompanyName.MyProjectName.Blazor.Server"},
            {"HttpApi", "MyCompanyName.MyProjectName.HttpApi.csproj"},
            {"HttpApi.Client", "MyCompanyName.MyProjectName.HttpApi.Client.csproj"},
            {"Web.Host", "MyCompanyName.MyProjectName.Web.Host.csproj"},
            {"Web", "MyCompanyName.MyProjectName.Web.csproj"}
        };

        foreach (var project in projects)
        {
            var reference = $@"..\..\..\..\..\lepton-theme\src\Volo.Abp.LeptonTheme.Management.{project.Key}\Volo.Abp.LeptonTheme.Management.{project.Key}.csproj";
            var projectFile = context.Files.FirstOrDefault(f => !f.Name.Contains("Test") && f.Name.Contains(project.Value) && f.Name.Contains(".csproj"));
            if (projectFile is null)
            {
                continue;
            }
            
            projects[project.Key] = projectFile.Name;
            
            AddProjectReference(projectFile, reference);

            AddModuleDependency(ConvertProjectFileToModuleFile(context, projectFile), 
                $"LeptonThemeManagement{ConvertProjectNameToModuleName(project.Key)}Module",
                underManagementFolder: project.Key != "HttpApi");
        }
    }

    private void AddModuleDependency(FileEntry moduleFile, string dependency, bool underManagementFolder = true)
    {
        var lines = moduleFile.GetLines();

        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("namespace MyCompanyName.MyProjectName"))
            {
                lines[i - 1] = lines[i - 1] + "using Volo.Abp.LeptonTheme" + (underManagementFolder ? ".Management" : "").EnsureEndsWith(';') + Environment.NewLine;
            }
            
            if (lines[i].Contains("public class MyProjectName") && lines[i-1].Contains(")]"))
            {
                lines[i - 2] = lines[i - 2] + "," + Environment.NewLine + $"\ttypeof({dependency})";
            }
        }
        
        moduleFile.SetLines(lines);
    }

    protected void ReplacePackageReferenceWithProjectReference(        
        ProjectBuildContext context, 
        string targetProjectFilePath,
        string packageReference,
        string projectReference)
    {
        var file = context.FindFile(targetProjectFilePath);
        if (file == null)
        {
            return;
        }
      
        file.NormalizeLineEndings();
        
        var lines = file.GetLines();
        var lineIndex = lines.FindIndex(line => line.Contains("PackageReference") && line.Contains(packageReference));
        if (lineIndex == -1)
        {
            return;
        }
        
        lines[lineIndex] = lines[lineIndex].Replace(lines[lineIndex], $"\t<ProjectReference Include=\"{projectReference}\" />");
        file.SetLines(lines);
    }

    protected void ChangeNamespaceAndKeyword(
        ProjectBuildContext context,
        string targetModuleFilePath,
        string oldNamespace,
        string newNamespace,
        string oldKeyword,
        string newKeyword)
    {
        var file = context.FindFile(targetModuleFilePath);

        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines();

        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains($"using {oldNamespace}"))
            {
                lines[i] = $"using {newNamespace};";
            }
            else if (lines[i].Contains(oldKeyword))
            {
                lines[i] = lines[i].Replace(oldKeyword, newKeyword);
            }
        }

        file.SetLines(lines);
    }

    protected void ChangeNamespace(
        ProjectBuildContext context,
        string targetModuleFilePath,
        string oldNamespace,
        string newNamespace)
    {
        var file = context.FindFile(targetModuleFilePath);

        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();

        var lines = file.GetLines();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains($"using {oldNamespace};"))
            {
                lines[i] = $"using {newNamespace};";
            }
        }

        file.SetLines(lines);
    }

    protected void ReplaceImportPackage(
        ProjectBuildContext context,
        string filePath,
        string oldImportPackage,
        string newImportPackage)
    {
        var file = context.FindFile(filePath);

        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();
        var lines = file.GetLines();
        var lineIndex = lines.FindIndex(line => line.Contains($"from '{oldImportPackage}'"));
        if (lineIndex == -1)
        {
            return;
        }
        
        lines[lineIndex] = lines[lineIndex].Replace(oldImportPackage, newImportPackage);
        file.SetLines(lines);
    }

    protected void RemoveLinesByStatement(
        ProjectBuildContext context,        
        string filePath,
        string statement)
    {
        var file = context.FindFile(filePath);

        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();
        
        var lines = file.GetLines();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains(statement))
            {
                lines[i] = null;
            }
        }

        file.SetLines(lines.Where(x => x != null));
    }

    protected void ReplaceMethodNames(
        ProjectBuildContext context,        
        string filePath,
        string oldMethodName,
        string newMethodName)
    {
        var file = context.FindFile(filePath);

        if (file == null)
        {
            return;
        }

        file.NormalizeLineEndings();
        
        var lines = file.GetLines();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains(oldMethodName))
            {
                lines[i] = lines[i].Replace(oldMethodName, newMethodName);
            }
        }
        
        file.SetLines(lines);
    }
    
    private static void AddProjectReference(FileEntry file, string reference)
    {
        if (!file.Name.Contains(".csproj"))
        {
            return;
        }
        
        var doc = new XmlDocument() { PreserveWhitespace = true };
        using (var stream = StreamHelper.GenerateStreamFromString(file.Content))
        {
            doc.Load(stream);

            var itemGroupNodes = doc.SelectNodes("/Project/ItemGroup");
            XmlNode itemGroupNode = null;

            if (itemGroupNodes == null || itemGroupNodes.Count < 1)
            {
                var projectNodes = doc.SelectNodes("/Project");
                var projectNode = projectNodes![0];

                itemGroupNode = doc.CreateElement("ItemGroup");
                projectNode.AppendChild(itemGroupNode);
            }
            else
            {
                for (var i = 0; i < itemGroupNodes.Count; i++)
                {
                    if (itemGroupNode is not null)
                    {
                        break;
                    }
                    for (var j = 0; j < itemGroupNodes[i].ChildNodes.Count; j++)
                    {
                        if (itemGroupNodes[i].ChildNodes[j].Name != "ProjectReference" || itemGroupNodes[i].ChildNodes[j].NodeType != XmlNodeType.Element)
                        {
                            continue;
                        }

                        itemGroupNode = itemGroupNodes[i];
                        break;
                    }
                }
            }

            itemGroupNode ??= itemGroupNodes[0];

            var packageReferenceNode = doc.CreateElement("ProjectReference");

            var includeAttr = doc.CreateAttribute("Include");
            includeAttr.Value = reference;
            packageReferenceNode.Attributes.Append(includeAttr);

            itemGroupNode.AppendChild(doc.CreateTextNode("\t"));
            itemGroupNode.AppendChild(packageReferenceNode);
            itemGroupNode.AppendChild(doc.CreateTextNode(Environment.NewLine));
            file.SetContent(doc.OuterXml);
        }
    }
    
    private static FileEntry ConvertProjectFileToModuleFile(ProjectBuildContext context, FileEntry projectFile)
    {
        var splittedProjectFileName = projectFile.Name.RemovePostFix("/").Split("/");
        var fileName = splittedProjectFileName.Last();
        
        splittedProjectFileName = splittedProjectFileName.Take(splittedProjectFileName.Length - 1).ToArray();
        
        fileName = fileName
            .Replace("MyCompanyName.", "")
            .Replace(".csproj", "Module")
            .Replace(".", "");

        fileName = ConvertProjectNameToModuleName(fileName);

        return context.Files.First(f => f.Name.Contains(splittedProjectFileName.Last() + "/" + fileName));
    }
    
    private static string ConvertProjectNameToModuleName(string moduleName)
    {
        moduleName = moduleName.Replace(".", "");
        
        return moduleName;
    }
    
    private static void RenameLeptonXFolders(ProjectBuildContext context, string folderName)
    {
        var leptonXFiles = context.Files.Where(x => x.Name.Contains("LeptonX") && x.IsDirectory);
        foreach (var file in leptonXFiles)
        {
            new MoveFolderStep(file.Name, file.Name.Replace("LeptonX", folderName)).Execute(context);
        }
    }
}