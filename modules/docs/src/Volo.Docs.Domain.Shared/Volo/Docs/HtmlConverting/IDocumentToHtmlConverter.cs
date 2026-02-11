using Volo.Docs.Documents;
using Volo.Docs.HtmlConverting;

namespace Volo.Docs.HtmlConverting;

public interface IDocumentToHtmlConverter<in TContext>
{
    string Convert(TContext context);
}
