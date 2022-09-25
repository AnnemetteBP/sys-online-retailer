using SharedModels.Converters;
using CustomerShared = SharedModels.Customer;
using CustomerHidden = CustomerApi.Models.Customer;

namespace CustomerApi.Models
{
    public class CustomerConverter : IConverter<CustomerHidden, CustomerShared>
    {
        public CustomerHidden Convert(CustomerShared customerPublic)
        {
            return new CustomerHidden
            {
                Id = customerPublic.Id,
                Name = customerPublic.Name,
                Email = customerPublic.Email,
                Phone = customerPublic.Phone,
                BillingAddress = customerPublic.BillingAddress,
                ShippingAddress = customerPublic.ShippingAddress
            };
        }

        public CustomerShared Convert(CustomerHidden customer)
        {
            return new CustomerShared
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                BillingAddress = customer.BillingAddress,
                ShippingAddress = customer.ShippingAddress
            };
        }

        public IEnumerable<CustomerHidden> ConvertAll(IEnumerable<CustomerShared> models)
        {
            var convertedModels = new List<CustomerHidden>();
            models.ToList().ForEach(model => convertedModels.Add(Convert(model)));
            return convertedModels;
        }

        public IEnumerable<CustomerShared> ConvertAll(IEnumerable<CustomerHidden> models)
        {
            var convertedModels = new List<CustomerShared>();
            models.ToList().ForEach(model => convertedModels.Add(Convert(model)));
            return convertedModels;
        }
    }
}

