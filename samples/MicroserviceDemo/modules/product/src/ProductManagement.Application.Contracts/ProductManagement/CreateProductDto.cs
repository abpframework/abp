namespace ProductManagement
{
    public class CreateProductDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public int StockCount { get; set; }
    }
}