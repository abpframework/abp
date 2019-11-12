using Volo.Abp.Http;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Views.Error
{
    public class AbpErrorViewModel
    {
        public RemoteServiceErrorInfo ErrorInfo { get; set; }

        public int HttpStatusCode { get; set; }
    }
}
