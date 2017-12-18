namespace System
{
    public static class AbpTypeExtensions
    {
        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }
    }
}
