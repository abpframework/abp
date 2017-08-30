using System;

namespace Volo.Abp.Http
{
    public class HttpVerbHelper //TODO: Internal?
    {
        public const string DefaultHttpVerb = "POST";

        public static string GetConventionalVerbForMethodName(string methodName)
        {
            if (methodName.StartsWith("Get", StringComparison.OrdinalIgnoreCase))
            {
                return "GET";
            }

            if (methodName.StartsWith("Put", StringComparison.OrdinalIgnoreCase) ||
                methodName.StartsWith("Update", StringComparison.OrdinalIgnoreCase))
            {
                return "PUT";
            }

            if (methodName.StartsWith("Delete", StringComparison.OrdinalIgnoreCase) ||
                methodName.StartsWith("Remove", StringComparison.OrdinalIgnoreCase))
            {
                return "DELETE";
            }

            if (methodName.StartsWith("Patch", StringComparison.OrdinalIgnoreCase))
            {
                return "PATCH";
            }

            if (methodName.StartsWith("Post", StringComparison.OrdinalIgnoreCase) ||
                methodName.StartsWith("Create", StringComparison.OrdinalIgnoreCase) ||
                methodName.StartsWith("Insert", StringComparison.OrdinalIgnoreCase))
            {
                return "POST";
            }

            //Default
            return DefaultHttpVerb;
        }
    }
}
