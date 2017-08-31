using System.Reflection;

namespace System
{
    public static class AbpTypeExtensions
    {
        //TODO: This method can be removed because not needed anymore!
        public static Assembly GetAssembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }
    }
}
