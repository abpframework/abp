using System.Collections.Generic;

namespace Volo.Abp.Data
{
    public class ConnectionStringsDictionary : Dictionary<string, string>
    {
        private const string DefaultConnectionStringName = "Default";

        public string Default
        {
            get { return this[DefaultConnectionStringName]; }
            set { this[DefaultConnectionStringName] = value; }
        }
    }
}