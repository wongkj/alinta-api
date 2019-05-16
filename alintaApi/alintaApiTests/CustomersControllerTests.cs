using System;
using alintaApi.Controllers;
using alintaApi.Models;
using alintaApi.Data;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace alintaApiTests
{
    public class CustomersControllerTests
    {
        protected readonly CustomersDbContext _context;

        public CustomersControllerTests()
        {
            // Instantiating the DbContext object.
            // With each test a new instance of the DbContext object is created.
            var options = new DbContextOptionsBuilder<CustomersDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new CustomersDbContext(options);

            _context.Database.EnsureCreated();
            
            // The prepopulated customers of our Data Store.
            var customers = new[]
            {
                    new Customer { Id = 1, firstName = "Steve", lastName = "Rogers", dateOfBirth = new DateTime(1910, 1, 1) },
                    new Customer { Id = 2, firstName = "Tony", lastName = "Stark", dateOfBirth = new DateTime(1970, 2, 2) },
                    new Customer { Id = 3, firstName = "Bruce", lastName = "Banner", dateOfBirth = new DateTime(1975, 3, 3) },
                    new Customer { Id = 4, firstName = "Peter", lastName = "Parker", dateOfBirth = new DateTime(2002, 4, 4) }
            };

            _context.Customers.AddRange(customers);
            _context.SaveChanges();

            var controller = new CustomersController(_context);
            var query = new CustomersController(_context);
        }

        /// <summary>
        ///     Check to see if the correct number of items is being returned 
        ///     from our Get Request.
        /// </summary>
        [Fact]
        public void Get_CheckCountOfGet_ReturnCount()
        {
            var controller = new CustomersController(_context);
            var count = _context.Customers.CountAsync().Result;
            var result = controller.Get(sort: "asc").CountAsync();
            Assert.Equal(count, result.Result);
        }

        /// <summary>
        ///     Check to see if 
        /// </summary>
        /// <param name="id">
        ///     The Id of the Customer we want to test.
        /// </param>
        /// <param name="firstName">
        ///     The First Name of the customer we want to test.
        /// </param>
        [Theory]
        [InlineData(1, "Steve")]
        [InlineData(2, "Tony")]
        [InlineData(3, "Bruce")]
        [InlineData(4, "Peter")]
        public void Get_GetCustomerByName_ReturnName(int id, string firstName)
        {
            var controller = new CustomersController(_context);
            var result = controller.Get(id);

            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as alintaApi.Models.Customer;
            var actual = model.firstName;
            Assert.Equal(firstName, actual);
        }

        /// <summary>
        ///     Check that when the Post Request is successful, then 
        ///     the Return Type is an OkObjectResult and the new Customer we posted.
        /// </summary>
        [Fact]
        public void Post_CheckReturnType_ReturnIActionResult()
        {
            var controller = new CustomersController(_context);
            var result = controller.Post(new Customer { Id = 7, firstName = "Natasha", lastName = "Romanov", dateOfBirth = new DateTime(1985, 05, 05) });
            Assert.IsType<OkObjectResult>(result);
        }

        /// <summary>
        ///     Check to see if the correct number of customers in our Data Store is
        ///     the Count of our Data Store plus 1 more customer.
        /// </summary>
        [Fact]
        public void Post_CheckCountAfterPost_ReturnCount()
        {
            var controller = new CustomersController(_context);
            var count = _context.Customers.CountAsync().Result;
            controller.Post(new Customer { Id = 7, firstName = "Natasha", lastName = "Romanov", dateOfBirth = new DateTime(1985, 05, 05) });
            var result = controller.Get(sort: "asc").CountAsync();
            Assert.Equal(count + 1, result.Result);
        }

        /// <summary>
        ///     Checks to see if the Last Name of the updated Customer
        ///     is still the same.
        /// </summary>
        [Fact]
        public void Put_CheckLastNameAfterChange_ReturnLastName()
        {
            var controller = new CustomersController(_context);
            var result = controller.Put(1, new Customer { Id = 7, firstName = "Steve", lastName = "Jackson", dateOfBirth = new DateTime(1910, 1, 1) });

            var okObjectResult = result as OkObjectResult;
            var model = okObjectResult.Value as alintaApi.Models.Customer;
            var actual = model.lastName;
            Assert.Equal("Jackson", actual);
        }

        /// <summary>
        ///     Checks to see how many items are in the Data Source after the Delete
        ///     method has been executed.
        /// </summary>
        [Fact]
        public void Delete_CheckCountAfterDelete_ReturnCount()
        {
            var controller = new CustomersController(_context);
            var count = _context.Customers.CountAsync().Result;
            controller.Delete(1);
            var result = controller.Get(sort: "asc").CountAsync();
            Assert.Equal(count - 1, result.Result);
        }

        /// <summary>
        ///     Disposing of the Data Store once it's been used.
        /// </summary>
        [Fact]
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
