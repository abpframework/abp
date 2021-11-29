using System;

namespace Volo.Abp.AspNetCore.Mvc.ModelBinding
{
    public static class ExtraPropertyBindingHelper
    {
        /// <summary>
        /// <paramref name="expression"/> is a string like "UserInfo.ExtraProperties[SocialSecurityNumber]"
        /// This method returns "SocialSecurityNumber" for this example. */
        /// </summary>
        public static string ExtractExtraPropertyName(string expression)
        {
            var index = expression.IndexOf("ExtraProperties[", StringComparison.Ordinal);
            if (index < 0)
            {
                return null;
            }

            return expression.Substring(index + 16, expression.Length - index - 17);
        }

        /// <summary>
        /// <paramref name="expression"/> is a string like "UserInfo.ExtraProperties[SocialSecurityNumber]"
        /// This method returns "UserInfo" for this example.
        /// </summary>
        public static string ExtractContainerName(string expression)
        {
            var index = expression.IndexOf("ExtraProperties[", StringComparison.Ordinal);
            if (index < 0)
            {
                return null;
            }

            return expression.Left(index).TrimEnd('.');
        }
    }
}
