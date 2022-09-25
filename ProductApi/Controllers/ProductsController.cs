using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Data;
using ProductApi.Models;
using Product = SharedModels.Product;
using ProductHidden = ProductApi.Models.Product;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<ProductHidden> repository;

        public ProductsController(IRepository<ProductHidden> repos)
        {
            repository = repos;
        }

        // GET products
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return new ProductConverter().ConvertAll(repository.GetAll());
        }

        // GET products/5
        [HttpGet("{id}", Name="GetProduct")]
        public IActionResult Get(int id)
        {
            var product = repository.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            var response = new ProductConverter().Convert(product);
            return new ObjectResult(response);
        }

        // POST products
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            if(product.Id != null)
            {
                product.Id = null;
            }

            var newProduct = repository.Add(new ProductConverter().Convert(product));

            return CreatedAtRoute("GetProduct", new { id = newProduct.Id }, newProduct);
        }

        // PUT products/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            if (product == null || product.Id != id)
            {
                return BadRequest();
            }

            var modifiedProduct = repository.Get(id);

            if (modifiedProduct == null)
            {
                return NotFound();
            }

            modifiedProduct.Name = product.Name;
            modifiedProduct.Price = product.Price;
            modifiedProduct.ItemsInStock = product.ItemsInStock;
            modifiedProduct.ItemsReserved += product.ItemsReserved;

            repository.Edit(modifiedProduct);
            return new ObjectResult(new ProductConverter().Convert(modifiedProduct));
        }

        // DELETE products/5
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
