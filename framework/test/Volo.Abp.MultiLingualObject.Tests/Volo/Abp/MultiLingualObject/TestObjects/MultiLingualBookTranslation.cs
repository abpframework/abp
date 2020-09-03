using System;

namespace Volo.Abp.MultiLingualObject.TestObjects
{
    public class MultiLingualBookTranslation : IMultiLingualTranslation<MultiLingualBook, Guid>
    {
        public string Name { get; set; }

        public string Language { get; set; }

        public MultiLingualBook Core { get; set; }

        public Guid CoreId { get; set; }
    }
}
