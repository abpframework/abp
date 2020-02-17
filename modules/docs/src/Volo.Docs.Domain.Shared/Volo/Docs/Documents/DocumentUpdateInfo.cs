using System;

namespace Volo.Docs.Documents
{
    [Serializable]
    public class DocumentUpdateInfo
    {
        public virtual string Name { get; set; }

        public virtual DateTime LastUpdatedTime { get; set; }

        public virtual int UpdatedCount { get; set; }
    }
}