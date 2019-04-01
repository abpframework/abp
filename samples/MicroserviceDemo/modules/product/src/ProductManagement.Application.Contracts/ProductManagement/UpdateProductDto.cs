using System.ComponentModel.DataAnnotations;

namespace ProductManagement
{
    public class UpdateProductDto
    {
        [Required]
        [StringLength(ProductConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(ProductConsts.MaxImageNameLength)]
        public string ImageName { get; set; }

        public float Price { get; set; }

        public int StockCount { get; set; }
    }
}