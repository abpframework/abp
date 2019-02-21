using System;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace ProductManagement
{
    [Serializable]
    public class ProductStockCountChangedEto : EtoBase
    {
        public Guid Id { get; }

        public int OldCount { get; set; }

        public int CurrentCount { get; set; }

        private ProductStockCountChangedEto()
        {
            //Default constructor is needed for deserialization.
        }

        public ProductStockCountChangedEto(Guid id, int oldCount, int currentCount)
        {
            Id = id;
            OldCount = oldCount;
            CurrentCount = currentCount;
        }
    }
}