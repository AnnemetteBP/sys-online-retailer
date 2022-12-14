using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using CustomerApi.Models;

namespace CustomerApi.Data
{
    public class DbInitializer : IDbInitializer
    {
        // This method will create and seed the database.
        public void Initialize(CustomerApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Products
            if (context.Customers.Any())
            {
                //return;   // DB has been seeded
            }

            List<Customer> customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Name", Email = "email@email.com", Phone = "00000000", BillingAddress = "Some Address", ShippingAddress = "Antoher Address" },
                new Customer { Id = 2, Name = "Test", Email = "test@test.com", Phone = "11111111", BillingAddress = "Test Address", ShippingAddress = "Antoher Test Address" }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
