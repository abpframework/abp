using System;
using Volo.Abp.Reflection;

namespace Acme.BookStore.Permissions
{
    public static class BookStorePermissions
    {
        public const string GroupName = "BookStore";

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";

        public static string[] GetAll()
        {
            //Return an array of all permissions
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(BookStorePermissions));
        }
    }
}