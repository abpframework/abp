using Volo.Abp;
using Volo.Abp.ExceptionHandling;

namespace Volo.Docs
{
    public class DocumentNotFoundException : BusinessException, IHasHttpStatusCode
    {
        public string DocumentUrl { get; set; }

        public int HttpStatusCode => (int)System.Net.HttpStatusCode.NotFound;

        public DocumentNotFoundException(string documentUrl)
            : base(DocsDomainErrorCodes.DocumentNotFound)
        {
            DocumentUrl = documentUrl;
        }
    }
}
