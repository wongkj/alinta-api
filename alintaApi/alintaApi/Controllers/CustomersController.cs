using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using alintaApi.Data;
using alintaApi.Models;
using System.Text.RegularExpressions;
using System.Diagnostics;

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
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IQueryable<Customer> Get(string sort)
        {
            try
            {
                IQueryable<Customer> customers;
                switch (sort)
                {
                    case "desc":
                        customers = _customersDbContext.Customers.OrderByDescending(c => c.firstName);
                        break;
                    case "asc":
                        customers = _customersDbContext.Customers.OrderBy(c => c.firstName);
                        break;
                    default:
                        customers = _customersDbContext.Customers;
                        break;
                }
                //  return Ok(customers);
                return customers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        // GET: api/Customers/5
        [HttpGet("{id}", Name = "Get")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IActionResult Get(int id)
        {
            try
            {
                var customer = _customersDbContext.Customers.Find(id);

                if (customer == null)
                {
                    return NotFound($"No Record found with id of {id}...");
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet("[action]")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IActionResult PagingCustomers(int? pageNumber, int? pageSize)
        {
            try
            {
                var customers = _customersDbContext.Customers;

                var currentPageNumber = pageNumber ?? 1;
                var currentPageSize = pageSize ?? 3;

                return Ok(customers.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("[action]")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IActionResult SearchByName(string firstName = "", string lastName = "")
        {
            try
            {
                var customers = _customersDbContext.Customers.Where(c => c.firstName.Contains(firstName) && c.lastName.Contains(lastName));

                if (customers.Count() == 0)
                {
                    return NotFound("No customers found with given search parameters...");
                }
                else
                {
                    return Ok(customers);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        // POST: api/Customers
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            try
            {
                _customersDbContext.Customers.Add(customer);
                _customersDbContext.SaveChanges();

                // return Ok("New Customer created successfully...");
                return Ok(customer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Customer customer)
        {
            try
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

                    return Ok(entity);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
