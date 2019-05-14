using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using alintaApi.Data;
using alintaApi.Models;

namespace alintaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private CustomersDbContext _customersDbContext;

        public CustomersController(CustomersDbContext customersDBContext)
        {
            _customersDbContext = customersDBContext;
        }

        // GET: api/Customers
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_customersDbContext.Customers);
        }

        // GET: api/Customers/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var customer = _customersDbContext.Customers.Find(id);

            if (customer == null)
            {
                return NotFound($"No Record found with id of {id}...");
            }

            return Ok(customer);
        }

        // POST: api/Customers
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            _customersDbContext.Customers.Add(customer);
            _customersDbContext.SaveChanges();

            return Ok("New Customer created successfully...");
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Customer customer)
        {
            var entity = _customersDbContext.Customers.Find(id);

            if (entity == null)
            {
                return NotFound($"Update Unsuccessful. No record found with id of {id}...");
            }
            else
            {
                entity.firstName = customer.firstName;
                entity.lastName = customer.lastName;
                entity.dateOfBirth = customer.dateOfBirth;
                _customersDbContext.SaveChanges();

                return Ok("Record Updated Successfully...");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var customer = _customersDbContext.Customers.Find(id);

            if (customer == null)
            {
                return NotFound($"Deletion Unsuccessful. No record found with id of {id}...");
            }
            else
            {
                _customersDbContext.Customers.Remove(customer);
                _customersDbContext.SaveChanges();

                return Ok("Customer deleted successfully...");
            }
        }
    }
}
