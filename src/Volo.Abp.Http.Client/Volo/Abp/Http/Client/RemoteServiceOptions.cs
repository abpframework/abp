using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Http.Client
{
    public class RemoteServiceOptions
    {
        public RemoteServiceDictionary RemoteServices { get; set; }

        public RemoteServiceOptions()
        {
            RemoteServices = new RemoteServiceDictionary();
        }
    }

    public class RemoteServiceDictionary : Dictionary<string, RemoteServiceConfiguration>
    {
        public const string DefaultName = "Default";

        public RemoteServiceConfiguration Default
        {
            get { return this.GetOrDefault(DefaultName); }
            set { this[DefaultName] = value; }
        }
    }

    public class RemoteServiceConfiguration
    {
        public string BaseUrl { get; set; }

        public RemoteServiceConfiguration()
        {
            
        }

        public RemoteServiceConfiguration(string baseUrl)
        {
            BaseUrl = baseUrl;
        }
    }
}
