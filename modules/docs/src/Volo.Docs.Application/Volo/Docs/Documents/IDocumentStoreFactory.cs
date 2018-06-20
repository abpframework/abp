using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public interface IDocumentStoreFactory
    {
        IDocumentStore Create(Project project);
    }
}