using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.DevExtremeAspNetData;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Globalize;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JsZip;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.DevExtreme
{
    [DependsOn(typeof(JsZipScriptContributor))]
    [DependsOn(typeof(GlobalizeScriptContributor))]
    [DependsOn(typeof(DevExtremeAspNetDataScriptContributor))]
    public class DevExtremeScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/devextreme/dist/js/dx.all.js");

            var options = context.ServiceProvider.GetRequiredService<IOptions<DevExtremeScriptBundleOptions>>()
                .Value;

            if (options.ScriptType.IncludeData)
            {
                context.Files.AddIfNotContains("/libs/devextreme-aspnet-data/js/dx.aspnet.data.js");
            }
            else if (options.ScriptType.IncludeMvcHelpers)
            {
                context.Files.AddIfNotContains("/libs/devextreme-aspnet-data/js/dx.aspnet.data.js");
                context.Files.AddIfNotContains("/libs/devextreme/aspnet.js");
            }
            else
            {
                throw new AbpException("Unknown DevExtremeScriptTypes: " + options.ScriptType);
            }

            if (options.AllowClientSideExporting) context.Files.AddIfNotContains("/libs/jszip/jszip.js");

            if (options.UseGlobalize)
            {
                context.Files.AddIfNotContains("/libs/cldr/cldr.js");
                context.Files.AddIfNotContains("/libs/cldr/event.js");
                context.Files.AddIfNotContains("/libs/cldr/supplemental.js");
                context.Files.AddIfNotContains("/libs/cldr/unresolved.js");
                context.Files.AddIfNotContains("/libs/globalize/globalize.js");
                context.Files.AddIfNotContains("/libs/globalize/message.js");
                context.Files.AddIfNotContains("/libs/globalize/number.js");
                context.Files.AddIfNotContains("/libs/globalize/currency.js");
                context.Files.AddIfNotContains("/libs/globalize/date.js");
            }
        }
    }

    /// <summary>
    ///     Define the default bundle for DevExtreme
    /// </summary>
    public class DevExtremeScriptBundleOptions
    {
        /// <summary>
        ///     Sets what js files to use for bundling
        /// </summary>
        public DevExtremeScriptTypes ScriptType { get; set; } = new DevExtremeScriptTypes();

        /// <summary>
        ///     Enables the dxGrids to export to the client (Includes JsZip)
        /// </summary>
        public bool AllowClientSideExporting { get; set; } = true;

        /// <summary>
        ///     Allows dx controls to globalize the output to users region (Includes Globalize)
        /// </summary>
        public bool UseGlobalize { get; set; } = true;
    }

    public class DevExtremeScriptTypes
    {
        public bool IncludeMvcHelpers { get; }
        public bool IncludeData { get; }
        
        /// <summary>
        ///     Default ctor that enables just the base DevExtreme js files, not including the data js files.
        /// </summary>
        public DevExtremeScriptTypes()
        {
            IncludeData = false;
            IncludeMvcHelpers = false;
        }

        /// <summary>
        ///     Override to include js data files and/or MvcHelpers
        /// </summary>
        /// <param name="includeData"></param>
        /// <param name="includeMvcHelpers"></param>
        public DevExtremeScriptTypes(bool includeData, bool includeMvcHelpers)
        {
            IncludeData = includeData;
            IncludeMvcHelpers = includeMvcHelpers;
        }

        /// <summary>
        ///     Shortcut to enable MvcHelpers
        /// </summary>
        /// <param name="includeMvcHelpers"></param>
        public DevExtremeScriptTypes(bool includeMvcHelpers)
        {
            if (!includeMvcHelpers) return;
            IncludeData = true;
            IncludeMvcHelpers = true;
        }
    }
}