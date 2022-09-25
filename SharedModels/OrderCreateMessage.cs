using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class OrderCreateMessage
    {
        public int? CustomerId { get; set; }
        public IList<OrderLine> OrderLines { get; set; }
    }
}
