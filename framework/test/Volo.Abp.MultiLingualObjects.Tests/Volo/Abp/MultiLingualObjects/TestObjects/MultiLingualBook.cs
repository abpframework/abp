using System;
using Volo.Abp.Data;


namespace Volo.Abp.MultiLingualObjects.TestObjects
{
    public class MultiLingualBook :  IMultiLingualObject
    {
        public MultiLingualBook(Guid id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public Guid Id { get; }
        
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string DefaultCulture { get; set; }

        public TranslationDictionary Translations { get; set; }
    }
}
