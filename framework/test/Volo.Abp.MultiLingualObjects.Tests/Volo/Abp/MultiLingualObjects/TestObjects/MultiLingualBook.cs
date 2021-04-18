using System;
using System.Collections.Generic;

namespace Volo.Abp.MultiLingualObjects.TestObjects
{
    public class MultiLingualBook :  IMultiLingualObject<MultiLingualBookTranslation>
    {
        public MultiLingualBook(Guid id, decimal price)
        {
            Id = id;
            Price = price;
        }

        public Guid Id { get; }

        public decimal Price { get; set; }

        public ICollection<MultiLingualBookTranslation> Translations { get; set; }
    }
}
