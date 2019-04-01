using Volo.Abp;

namespace ProductManagement
{
    public class ProductCodeAlreadyExistsException : BusinessException
    {
        public ProductCodeAlreadyExistsException(string productCode)
            : base("PM:000001", $"A product with code {productCode} has already exists!")
        {

        }
    }
}