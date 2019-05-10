using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Volo.Abp.Cli.Utils
{
    public static class VersionHelper
    {
        //public static string Version => typeof(AbpCliCoreModule).Assembly
        //    .GetFileVersion()
        //    .RemovePostFix(".0");
        public static string Version => "0.17.0.0";
    }
}
