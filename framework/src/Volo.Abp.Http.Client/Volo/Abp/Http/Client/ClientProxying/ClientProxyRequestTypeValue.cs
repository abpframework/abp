using System;
using System.Collections;
using System.Collections.Generic;

namespace Volo.Abp.Http.Client.ClientProxying
{
    public class ClientProxyRequestTypeValue : IEnumerable<KeyValuePair<Type, object>>
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

        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
