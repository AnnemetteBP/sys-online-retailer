using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using CustomerApi.Models;
using Customer = CustomerApi.Models.Customer;

namespace CustomerApi.Data
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly CustomerApiContext db;

        public CustomerRepository(CustomerApiContext context)
        {
            db = context;
        }

        Customer IRepository<Customer>.Add(Customer entity)
        {
            /// TODO add checks for validity of new customer object
            
            var newOrder = db.Customers.Add(entity).Entity;
            db.SaveChanges();
            return newOrder;
        }

        void IRepository<Customer>.Edit(Customer entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        Customer IRepository<Customer>.Get(int id)
        {
            return db.Customers.FirstOrDefault(o => o.Id == id);
        }

        IEnumerable<Customer> IRepository<Customer>.GetAll()
        {
            return db.Customers.ToList();
        }

        void IRepository<Customer>.Remove(int id)
        {
            var order = db.Customers.FirstOrDefault(p => p.Id == id);
            db.Customers.Remove(order);
            db.SaveChanges();
        }
    }
}
