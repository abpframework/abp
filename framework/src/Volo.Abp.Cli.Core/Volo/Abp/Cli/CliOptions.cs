using System;
using System.Collections.Generic;

namespace Volo.Abp.Cli
{
    public class CliOptions
    {
        public Dictionary<string, Type> Commands { get; }

        /// <summary>
        /// Default value: true.
        /// </summary>
        public bool CacheTemplates { get; set; } = true;

        public CliOptions()
        {
            Commands = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        }
    }
}