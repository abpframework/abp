using System;
using System.Collections.Generic;

namespace Volo.Abp.Cli
{
    public class CliOptions
    {
        public Dictionary<string, Type> Commands { get; }

        public string AbpIoWwwUrlRoot { get; set; } = "https://abp.io/";

        public string AbpIoAccountUrlRoot { get; set; } = "https://account.abp.io/";

        public CliOptions()
        {
            Commands = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        }
    }
}