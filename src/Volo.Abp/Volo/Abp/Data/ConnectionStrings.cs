using System.Collections.Generic;

namespace Volo.Abp.Data
{
    public class ConnectionStrings : Dictionary<string, string>
    {
        public const string DefaultConnectionStringName = "Default";
        
        public string Default
        {
            get { return this[DefaultConnectionStringName]; }
            set { this[DefaultConnectionStringName] = value; }
        }
    }
}