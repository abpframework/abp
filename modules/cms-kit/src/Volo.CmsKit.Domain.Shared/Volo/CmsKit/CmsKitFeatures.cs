using System.Collections.Generic;

namespace Volo.CmsKit
{
    public static class CmsKitFeatures
    {
        public const string NamePrefix = "CmsKit";

        public static class Reactions
        {
            public const string Name = NamePrefix + ".Reactions";

            public static bool IsEnabled
            {
                get => GlobalFeatures.IsEnabled(Name);
                set => GlobalFeatures.SetEnabled(Name, value);
            }
        }

        public static class Comments
        {
            public const string Name = NamePrefix + ".Comments";

            public static bool IsEnabled
            {
                get => GlobalFeatures.IsEnabled(Name);
                set => GlobalFeatures.SetEnabled(Name, value);
            }
        }

        public static void EnableAll()
        {
            foreach (var featureName in GetAllNames())
            {
                GlobalFeatures.Enable(featureName);
            }
        }

        public static void DisableAll()
        {
            foreach (var featureName in GetAllNames())
            {
                GlobalFeatures.Disable(featureName);
            }
        }

        public static IEnumerable<string> GetAllNames()
        {
            return new[]
            {
                Reactions.Name,
                Comments.Name
            };
        }
    }
}
