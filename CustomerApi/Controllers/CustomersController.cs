using CustomerApi.Data;
using Microsoft.AspNetCore.Mvc;
using CustomerHidden = CustomerApi.Models.Customer;
using Customer = SharedModels.Customer;
using CustomerApi.Models;

namespace CustomerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IRepository<CustomerHidden> repository;

        public CustomersController(IRepository<CustomerHidden> repos)
        {
            repository = repos;
        }

        // GET customers
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return new CustomerConverter().ConvertAll(repository.GetAll());
        }

        // GET customers/5
        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult Get(int id)
        {
            Customer customer = new CustomerConverter().Convert(repository.Get(id));
            if (customer == null)
            {
                return NotFound();
            }
            return new ObjectResult(customer);
        }

        // POST customers
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }
            if(customer.Id != null)
            {
                customer.Id = null;
            }
            var newCustomer = repository.Add(new CustomerConverter().Convert(customer));

            return CreatedAtRoute("GetCustomer", new { id = newCustomer.Id }, newCustomer);
        }

        // PUT customers/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Customer customer)
        {
            if (customer == null || customer.Id == null)
            {
                return BadRequest();
            }

            var modifiedCustomer = repository.Get((int)customer.Id);

            if (modifiedCustomer == null)
            {
                return NotFound();
            }

            modifiedCustomer.Name = customer.Name;
            modifiedCustomer.Email = customer.Email;
            modifiedCustomer.Phone = customer.Phone;
            modifiedCustomer.BillingAddress = customer.BillingAddress;
            modifiedCustomer.ShippingAddress = customer.ShippingAddress;

            repository.Edit(modifiedCustomer);

            return new ObjectResult(modifiedCustomer);
        }

        // DELETE customers/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (repository.Get(id) == null)
            {
                return NotFound();
            }

            repository.Remove(id);
            return new ObjectResult(id);
        }
    }
}