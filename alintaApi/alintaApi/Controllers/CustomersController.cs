using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using alintaApi.Data;
using alintaApi.Models;
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

        /// <summary>
        ///     Get All the Customers within the DataSource.
        /// </summary>
        /// <param name="sort">
        ///     The order you want to arrange the Resulting data.
        /// </param>
        /// <returns>
        ///     An Object I can iterate over.
        /// </returns>
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
                return customers;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     Returns the result of a single Customer based on the Id specified.
        /// </summary>
        /// <param name="id">
        ///     The Id of the Customer you want returned.
        /// </param>
        /// <returns>
        ///     An OkObjectResult with a value of the Customer Object located.
        /// </returns>
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

        /// <summary>
        ///     Pages through the Customer data to return Customer data in chunks instead
        ///     of one large package.
        /// </summary>
        /// <param name="pageNumber">
        ///     The page number you want to return. This parameter is nullable.
        /// </param>
        /// <param name="pageSize">
        ///     The number of customers you want displayed on each page. This parameter is nullable.
        /// </param>
        /// <returns>
        ///     Returns a section of the data as per the requirements specified for the
        ///     pageNumber and pageSize.
        /// </returns>
        [HttpGet("[action]")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public IActionResult PagingCustomers(int? pageNumber, int? pageSize)
        {
            try
            {
                var customers = _customersDbContext.Customers;

                // If the pageNumber and pageSize are Not Null then the values passed in are the
                // values assigned to the currentPageNumber and currentPageSize variables.
                // Otherwise the default values of 1 and 3 are provided.
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

        /// <summary>
        ///     Search for the customers based on either their First Name, Last Name or both.
        /// </summary>
        /// <param name="firstName">
        ///     The First Name of the customer you wish to locate.
        /// </param>
        /// <param name="lastName">
        ///     The Last Name of the customer you wish to locate.
        /// </param>
        /// <returns>
        ///     Returns a list of customers that match the search criteria provided.
        /// </returns>
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

        /// <summary>
        ///     Add a new customer to the data store.
        /// </summary>
        /// <param name="customer">
        ///     A Customer object is required to add the new databse.
        /// </param>
        /// <returns>
        ///     An IActionResult as well as the customer object provided.
        /// </returns>
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            try
            {
                _customersDbContext.Customers.Add(customer);
                _customersDbContext.SaveChanges();

                return Ok(customer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     Updates one or more property values of a Customer object.
        /// </summary>
        /// <param name="id">
        ///     The Id of the customer you want to update.
        /// </param>
        /// <param name="customer">
        ///     The Customer object with the modified field/s.
        /// </param>
        /// <returns>
        ///     Returns an IActionResult and the NEW Customer object.
        /// </returns>
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
                    // Replacing the customer information in the data store with the information
                    // provided in the body of the PUT Request.
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

        /// <summary>
        ///     Removes a customer from the data source.
        /// </summary>
        /// <param name="id">
        ///     The Id of the customer you want to remove.
        /// </param>
        /// <returns>
        ///     An IActionResult of Ok.
        /// </returns>
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
