using SharedModels.Converters;
using CustomerProduct = SharedModels.Product;

namespace ProductApi.Models
{
    public class ProductConverter : IConverter<Product, CustomerProduct>
    {
        public Product Convert(CustomerProduct sharedProduct)
        {
            return new Product
            {
                Id = sharedProduct.Id,
                Name = sharedProduct.Name,
                Price = sharedProduct.Price,
                ItemsInStock = sharedProduct.ItemsInStock,
                ItemsReserved = sharedProduct.ItemsReserved
            };
        }

        public CustomerProduct Convert(Product hiddenProduct)
        {
            return new CustomerProduct
            {
                Id = hiddenProduct.Id,
                Name = hiddenProduct.Name,
                Price = hiddenProduct.Price,
                ItemsInStock = hiddenProduct.ItemsInStock,
                ItemsReserved = hiddenProduct.ItemsReserved
            };
        }

        public IEnumerable<Product> ConvertAll(IEnumerable<CustomerProduct> models)
        {
            var convertedModels = new List<Product>();
            models.ToList().ForEach(model => convertedModels.Add(Convert(model)));
            return convertedModels;
        }

        public IEnumerable<CustomerProduct> ConvertAll(IEnumerable<Product> models)
        {
            var convertedModels = new List<CustomerProduct>();
            models.ToList().ForEach(model => convertedModels.Add(Convert(model)));
            return convertedModels;
        }
    }
}
