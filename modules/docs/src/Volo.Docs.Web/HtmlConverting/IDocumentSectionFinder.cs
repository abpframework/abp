using Volo.Abp.DependencyInjection;

namespace Volo.Docs.HtmlConverting
{
    public interface IDocumentSectionFinder: ITransientDependency
    {
        public DocumentSectionDictionary Find(string document);
    }
}
