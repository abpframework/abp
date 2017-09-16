using System;
using Volo.Abp.Http;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class DefaultUrlActionNameNormalizer : IUrlActionNameNormalizer
    {
        public void Normalize(UrlActionNameNormalizerContext context)
        {
            if (context.ActionNameInUrl.IsNullOrEmpty())
            {
                return;
            }

            context.ActionNameInUrl = HttpMethodHelper.RemoveHttpMethodPrefix(context.ActionNameInUrl, context.HttpMethod);
        }
    }
}