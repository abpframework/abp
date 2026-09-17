using System.Collections.Generic;

namespace Volo.Docs.Documents.Rendering
{
    public class DocumentRenderParameters : Dictionary<string,string>
    {
        public DocumentRenderParameters()
        {
            
        }
        
        public DocumentRenderParameters(DocumentRenderParameters renderParameters)
        {
            foreach (var parameter in renderParameters)
            {
                Add(parameter.Key, parameter.Value);
            }
        }
    }
}