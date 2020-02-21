using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    /// <summary>
    /// Compares the server-side parameter-type with the local parameter-type considering differences between
    /// the .net-core and the traditional .net-framework.
    /// Inspired by 'https://github.com/akkadotnet/akka.net/pull/2947/files'
    /// </summary>
    public class CrossPlatformParameterTypeComparer : ParameterTypeComparer, ITransientDependency
    {
        const string Placeholder = "%COREFX%";
        const string NetCoreLib = "System.Private.CoreLib";
        const string NetFxLib = "mscorlib";

        static readonly bool runsOnNetFx;

        static CrossPlatformParameterTypeComparer()
        {
            runsOnNetFx = string.IsNullOrEmpty(EnvironmentHelper.RuntimeNetCoreVersion);
        }

        public override bool TypeMatches(MethodParameterApiDescriptionModel actionParameter, ParameterInfo methodParameter)
        {
            if (runsOnNetFx)
            {
                // Compare the server's DotnetCore type relative to the current, traditional framework type
                // by replacing framework assemblies with an placeholder string.
                return actionParameter.TypeAsString.Replace(NetCoreLib, Placeholder)
                    == methodParameter.ParameterType.GetFullNameWithAssemblyName().Replace(NetFxLib, Placeholder);
            }
            else
            {
                return base.TypeMatches(actionParameter, methodParameter);
            }
        }
    }
}
