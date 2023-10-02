using System.Collections.Generic;

namespace Volo.Docs.Documents.FullSearch.Elastic;

public class EsDocumentResult
{
    public EsDocumentResult()
    {
        EsDocuments = new List<EsDocument>();
    }
        
    public long TotalCount { get; set; }

    public List<EsDocument> EsDocuments { get; set; }
}