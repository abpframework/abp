namespace Volo.Docs.Documents
{
    public class NavigationTreePostProcessorContext
    {
        public DocumentWithDetailsDto Document { get; }
        public NavigationNode RootNode { get; }
        
        public NavigationTreePostProcessorContext(DocumentWithDetailsDto document, NavigationNode rootNode)
        {
            Document = document;
            RootNode = rootNode;
        }
    }
}