﻿using System;
using Volo.Abp.Reflection;

namespace MyCompanyName.MyProjectName.Permissions
{
    public static class MyProjectNamePermissions
    {
        public const string GroupName = "MyProjectName";

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";

        public static string[] GetAll()
        {
            //Return an array of all permissions
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(MyProjectNamePermissions));
        }
    }
}