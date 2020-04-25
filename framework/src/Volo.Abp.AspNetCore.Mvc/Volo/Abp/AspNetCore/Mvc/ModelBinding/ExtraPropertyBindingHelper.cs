using System;

namespace Volo.Abp.AspNetCore.Mvc.ModelBinding
{
    public static class ExtraPropertyBindingHelper
    {
        /// <summary>
        /// modelName is a string like "UserInfo.ExtraProperties[SocialSecurityNumber]"
        /// This method returns "SocialSecurityNumber" for this example. */
        /// </summary>
        public static string ExtractExtraPropertyName(string modelName)
        {
            var index = modelName.IndexOf(".ExtraProperties[", StringComparison.Ordinal);
            if (index < 0)
            {
                return null;
            }

            return modelName.Substring(index + 17, modelName.Length - index - 18);
        }

        /// <summary>
        /// modelName is a string like "UserInfo.ExtraProperties[SocialSecurityNumber]"
        /// This method returns "UserInfo" for this example.
        /// </summary>
        public static string ExtractContainerName(string modelName)
        {
            var index = modelName.IndexOf(".ExtraProperties[", StringComparison.Ordinal);
            if (index < 0)
            {
                return null;
            }

            return modelName.Left(index);
        }
    }
}
