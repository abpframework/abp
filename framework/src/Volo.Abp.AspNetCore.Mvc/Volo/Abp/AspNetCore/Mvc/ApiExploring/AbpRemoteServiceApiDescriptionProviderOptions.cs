using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring
{
    public class AbpRemoteServiceApiDescriptionProviderOptions
    {
        public HashSet<ApiResponseType> SupportedResponseTypes { get; set; } = new HashSet<ApiResponseType>();
    }
}