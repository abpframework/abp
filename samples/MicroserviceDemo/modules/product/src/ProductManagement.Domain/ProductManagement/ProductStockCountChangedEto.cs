using System;
using Volo.Abp.Domain.Entities.Events.Distributed;

namespace ProductManagement
{
    [Serializable]
    public class ProductStockCountChangedEto : EtoBase
    {
        public int OldCount { get; set; }

        public int CurrentCount { get; set; }

        private ProductStockCountChangedEto()
        {
            //Default constructor is needed for deserialization.
        }

        public ProductStockCountChangedEto(int oldCount, int currentCount)
        {
            OldCount = oldCount;
            CurrentCount = currentCount;
        }
    }
}