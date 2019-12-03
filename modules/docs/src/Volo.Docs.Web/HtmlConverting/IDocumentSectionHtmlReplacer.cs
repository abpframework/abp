using Volo.Abp.DependencyInjection;

namespace Volo.Docs.HtmlConverting
{
    public interface IDocumentSectionHtmlReplacer : ITransientDependency
    {
        public string Replace(string document);
    }
}
