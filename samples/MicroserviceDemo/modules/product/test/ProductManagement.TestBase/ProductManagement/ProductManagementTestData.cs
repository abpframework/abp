using System;
using Volo.Abp.DependencyInjection;

namespace ProductManagement
{
    public class ProductManagementTestData : ISingletonDependency
    {
        public Guid ProductId1 { get; } = Guid.NewGuid();

        public string ProductCode1 { get; } = "ProductCode1";

        public string ProductName1 { get; } = "ProductName1";

        public float ProductPrice1 { get; } = 20;

        public int ProductStockCount1 { get; } = 100;

        public Guid ProductId2 { get; } = Guid.NewGuid();

        public string ProductCode2 { get; } = "ProductCode2";

        public string ProductName2 { get; } = "ProductName2";

        public float ProductPrice2 { get; } = 30;

        public int ProductStockCount2 { get; } = 110;
    }
}
