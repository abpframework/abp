using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    /// <summary>
    /// Provide helpers to check the environment where the current ABP app is currently running
    /// </summary>
    static class EnvironmentHelper
    {
        /// <summary>
        /// Initializes the <see cref="EnvironmentHelper"/> class.
        /// </summary>
        static EnvironmentHelper()
        {
            RuntimeNetCoreVersion = GetNetCoreVersion();
        }

        /// <summary>
        /// Gets the runtime net core version.
        /// </summary>
        /// <remarks>
        /// If the <see cref="RuntimeNetCoreVersion"/> is null the app is running on a .net Classic environment
        /// </remarks>
        public static string RuntimeNetCoreVersion { get; }

        /// <summary>
        /// Gets the net core version.
        /// </summary>
        static string GetNetCoreVersion()
        {
            var assembly = typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly;
            var assemblyPath = assembly.CodeBase.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            int netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");
            if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2)
            {
                return assemblyPath[netCoreAppIndex + 1];
            }
            return null;
        }
    }
}
