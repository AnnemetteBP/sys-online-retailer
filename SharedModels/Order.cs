using System.Collections.Generic;

namespace SharedModels
{
    public class Order
    {
        public int CustomerId { get; set; }
        public IList<OrderLine> OrderLines { get; set; }
    }

    public class OrderLine
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}