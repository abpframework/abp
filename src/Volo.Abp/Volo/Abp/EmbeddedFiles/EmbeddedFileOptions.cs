using System.Collections.Generic;

namespace Volo.Abp.EmbeddedFiles
{
    public class EmbeddedFileOptions
    {
        public List<EmbeddedFileSet> Sources { get; }

        public EmbeddedFileOptions()
        {
            Sources = new List<EmbeddedFileSet>();
        }
    }
}