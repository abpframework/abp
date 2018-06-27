using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Blogging
{
    public class BloggingPermissions
    {
        public const string GroupName = "Blogging";

        public static class Blogs
        {
            public const string Default = GroupName + ".Blog";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
        }

        public static class Posts
        {
            public const string Default = GroupName + ".Post";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
        }

        public static string[] GetAll()
        {
            return new[]
            {
                GroupName,
                Blogs.Default,
                Blogs.Delete,
                Blogs.Update,
                Blogs.Create,
                Posts.Default,
                Posts.Delete,
                Posts.Update,
                Posts.Create
            };
        }
    }
}
