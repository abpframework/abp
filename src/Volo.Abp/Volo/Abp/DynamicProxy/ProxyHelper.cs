using System.Linq;
using System.Reflection;

namespace Volo.Abp.DynamicProxy
{
    public static class ProxyHelper
    {
        /// <summary>
        /// Returns dynamic proxy target object if this is a proxied object, otherwise returns the given object. 
        /// </summary>
        public static object UnProxy(object obj)
        {
            //TODO: This code depends on Castle, so we should find a better way.

            if (obj.GetType().Namespace != "Castle.Proxies")
            {
                return obj;
            }

            var targetField = obj.GetType().GetTypeInfo()
                .GetFields()
                .FirstOrDefault(f => f.Name == "__target");

            if (targetField == null)
            {
                return obj;
            }

            return targetField.GetValue(obj);
        }
    }
}
