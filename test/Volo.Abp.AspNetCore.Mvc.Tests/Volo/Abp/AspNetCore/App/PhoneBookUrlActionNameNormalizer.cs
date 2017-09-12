using System;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.App
{
    public class PhoneBookUrlActionNameNormalizer : IUrlActionNameNormalizer
    {
        public void Normalize(UrlActionNameNormalizerContext context)
        {
            if (string.Equals(context.ActionNameInUrl, "phone", StringComparison.OrdinalIgnoreCase))
            {
                context.ActionNameInUrl = "phones";
                return;
            }
        }
    }
}