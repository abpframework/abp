using System.Collections.Generic;
using System.Linq;

namespace Volo.CmsKit
{
    internal static class GlobalFeatures //TODO: Move to the ABP Framework..?
    {
        private static readonly HashSet<string> EnabledFeatures = new HashSet<string>();

        public static bool IsEnabled(string featureName)
        {
            return EnabledFeatures.Contains(featureName);
        }

        public static void SetEnabled(string featureName, bool isEnabled)
        {
            if (isEnabled)
            {
                Enable(featureName);
            }
            else
            {
                Disable(featureName);
            }
        }

        public static void Enable(string featureName)
        {
            EnabledFeatures.AddIfNotContains(featureName);
        }

        public static void Disable(string featureName)
        {
            EnabledFeatures.Remove(featureName);
        }

        public static IEnumerable<string> GetEnabledFeatures()
        {
            return EnabledFeatures;
        }
    }
}
