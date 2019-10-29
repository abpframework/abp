using Volo.Abp.Reflection;

namespace Acme.BookStore.BookManagement.Authorization
{
    public class BookManagementPermissions
    {
        public const string GroupName = "BookManagement";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(BookManagementPermissions));
        }
    }
}