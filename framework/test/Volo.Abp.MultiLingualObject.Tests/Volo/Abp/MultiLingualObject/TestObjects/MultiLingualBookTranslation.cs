using System;

namespace Volo.Abp.MultiLingualObject.TestObjects
{
    public class MultiLingualBookTranslation : IMultiLingualTranslation
    {
        public string Name { get; set; }

        public string Language { get; set; }
    }
}
