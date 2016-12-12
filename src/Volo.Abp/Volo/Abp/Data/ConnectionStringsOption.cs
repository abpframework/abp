using System.Collections.Generic;

namespace Volo.Abp.Data
{
    public class ConnectionStringsOption
    {
        public string Default { get; set; }

        public Dictionary<string, string> Modules { get; set; }

        public ConnectionStringsOption()
        {
            Modules = new Dictionary<string, string>();
        }
    }
}