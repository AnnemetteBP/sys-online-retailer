using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;
using OrderApi.Models;
using RestSharp;
using OrderHidden = OrderApi.Models.Order;
using Order = SharedModels.Order;
using OrderLineHidden = OrderApi.Models.OrderLine;
using OrderLine = SharedModels.OrderLine;
using Product = SharedModels.Product;
using Customer = SharedModels.Customer;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IRepository<OrderHidden> repository;

        public OrdersController(IRepository<OrderHidden> repos)
        {
            repository = repos;
        }

        // GET: orders
        [HttpGet("GetAllByCustomer/{customerId}", Name = "GetAllByCustomer")]
        public IEnumerable<Order> GetAllByCustomer(int customerId)
        {
            return new OrderConverter().ConvertAll(repository.GetAll().Where(o => o.CustomerId == customerId));
        }

        // GET orders/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST orders
        [HttpPost(Name = "Add")]
        public IActionResult Post([FromBody]Order order)
        {
            if (order == null)
            {
                return BadRequest("Order was invalid");
            }
            // Call CustomerApi to check if customer exists and have no unpaied orders (tentative)
            RestClient customerClient = new RestClient("http://customerapi/customers/");
            var customerRequest = new RestRequest(order.CustomerId.ToString());
            var customerResponse = customerClient.GetAsync<Customer>(customerRequest);
            try
            {
                customerResponse.Wait();
            }catch(AggregateException)
            {
                return BadRequest("Customer does not exist with id: " + order.CustomerId + ".");
            }
            Customer customer = customerResponse.Result;
            var foo = repository.GetAll();
            List<OrderHidden> orders = repository.GetAll().Where(o => o.CustomerId == customer.Id).ToList();
            if(orders.Any(o => o.Status == OrderHidden.OrderStatus.tentative))
            {
                return BadRequest("Customer credit standing is negative.");
            }
            // Call ProductApi to check if products exists and are in stock
            foreach(OrderLine orderLine in order.OrderLines)
            {
                RestClient productClient = new RestClient("http://productapi/products/");
                var productRequest = new RestRequest(orderLine.ProductId.ToString());
                var productResponse = productClient.GetAsync<Product>(productRequest);
                try
                {
                    productResponse.Wait();
                    var product = productResponse.Result;
                    if(product.ItemsInStock - product.ItemsReserved - orderLine.Quantity < 0)
                    {
                        return BadRequest("Product is not in stock with id: " + orderLine.ProductId + ".");
                    }
                    product.ItemsReserved += orderLine.Quantity;
                    var buyProductRequest = new RestRequest(orderLine.ProductId.ToString());
                    buyProductRequest.AddJsonBody<Product>(product);
                    var buyProductResponse = productClient.PutAsync<Product>(buyProductRequest);
                    try
                    {
                        buyProductResponse.Wait();
                        var response = buyProductResponse.Result;
                    }
                    catch (AggregateException)
                    {
                        return BadRequest("Product was not ordered with id: " + orderLine.ProductId + ".");
                    }
                }
                catch (AggregateException)
                {
                    return BadRequest("Product do not exist with id: " + orderLine.ProductId + ".");
                }
            }
            return new ObjectResult(order);
        }

    }
}
