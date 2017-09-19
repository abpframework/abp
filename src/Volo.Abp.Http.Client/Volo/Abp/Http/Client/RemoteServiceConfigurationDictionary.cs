using System.Collections.Generic;

namespace Volo.Abp.Http.Client
{
    public class RemoteServiceConfigurationDictionary : Dictionary<string, RemoteServiceConfiguration>
    {
        public const string DefaultName = "Default";

        public RemoteServiceConfiguration Default
        {
            get { return this.GetOrDefault(DefaultName); }
            set { this[DefaultName] = value; }
        }
    }
}