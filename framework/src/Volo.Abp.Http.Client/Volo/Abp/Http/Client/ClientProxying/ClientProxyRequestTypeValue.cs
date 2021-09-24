using System;
using System.Collections.Generic;

namespace Volo.Abp.Http.Client.ClientProxying
{
    public class ClientProxyRequestTypeValue
    {
        public List<KeyValuePair<Type, object>> Values { get; private set; }

        public ClientProxyRequestTypeValue()
        {
            Values = new List<KeyValuePair<Type, object>>();
        }

        public void Add(Type type, object value)
        {
            Values.Add(new KeyValuePair<Type, object>(type, value));
        }
    }
}
