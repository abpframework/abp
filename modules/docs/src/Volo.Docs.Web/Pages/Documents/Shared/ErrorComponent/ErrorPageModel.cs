namespace Volo.Docs.Pages.Documents.Shared.ErrorComponent
{
    public class ErrorPageModel
    {
        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

        public string RedirectUrl { get; set; }

        public bool AutoRedirect { get; set; } = true;
    }
}
