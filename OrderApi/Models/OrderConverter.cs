using CustomerOrder = SharedModels.Order;
using CustomerOrderLine = SharedModels.OrderLine;

namespace OrderApi.Models
{
    public class OrderConverter
    {
        public Order Convert(CustomerOrder model)
        {
            return new Order
            {
                CustomerId = model.CustomerId,
                OrderLines = ConvertAll(model.OrderLines).ToList()
            };
        }

        public CustomerOrder Convert(Order model)
        {
            return new CustomerOrder
            {
                CustomerId = model.CustomerId,
                OrderLines = ConvertAll(model.OrderLines).ToList()
            };
        }

        public IEnumerable<Order> ConvertAll(IEnumerable<CustomerOrder> models)
        {
            var convertedModels = new List<Order>();
            models.ToList().ForEach(model => convertedModels.Add(Convert(model)));
            return convertedModels;
        }

        public IEnumerable<CustomerOrder> ConvertAll(IEnumerable<Order> models)
        {
            var convertedModels = new List<CustomerOrder>();
            models.ToList().ForEach(model => convertedModels.Add(Convert(model)));
            return convertedModels;
        }

        public OrderLine Convert(CustomerOrderLine model)
        {
            return new OrderLine
            {
                ProductId = model.ProductId,
                Quantity = model.Quantity,
            };
        }

        public CustomerOrderLine Convert(OrderLine model)
        {
            return new CustomerOrderLine
            {
                ProductId = model.ProductId,
                Quantity = model.Quantity,
            };
        }

        public IEnumerable<OrderLine> ConvertAll(IEnumerable<CustomerOrderLine> models)
        {
            var convertedModels = new List<OrderLine>();
            models.ToList().ForEach(model => convertedModels.Add(Convert(model)));
            return convertedModels;
        }

        public IEnumerable<CustomerOrderLine> ConvertAll(IEnumerable<OrderLine> models)
        {
            var convertedModels = new List<CustomerOrderLine>();
            models.ToList().ForEach(model => convertedModels.Add(Convert(model)));
            return convertedModels;
        }
    }
}
