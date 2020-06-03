namespace Volo.Abp.Web.Http
{
    public static class AbpUrlHelper
    {
        public static bool IsLocalUrl(string url)
        {
            //This code is copied from Microsoft.AspNetCore.Mvc.Routing.UrlHelperBase class.

            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            if (url[0] == '/')
            {
                if (url.Length == 1)
                {
                    return true;
                }

                if (url[1] != '/' && url[1] != '\\')
                {
                    return true;
                }

                return false;
            }

            if (url[0] == '~' && url.Length > 1 && url[1] == '/')
            {
                if (url.Length == 2)
                {
                    return true;
                }

                if (url[2] != '/' && url[2] != '\\')
                {
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
