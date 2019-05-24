using System;
using System.Collections.Generic;

namespace Volo.Abp.Cli
{
    public class CliOptions
    {
        public Dictionary<string, Type> Commands { get; }

        public CliOptions()
        {
            Commands = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        }
    }
}