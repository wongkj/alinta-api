using System;
using alintaApi.Controllers;
using alintaApi.Models;
using alintaApi.Data;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Autofac.Extras.Moq;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace alintaApiTests
{
    public class CustomersControllerTests
    {
        protected readonly CustomersDbContext _context;
        // private CustomersController controller;

        public CustomersControllerTests()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new CustomersDbContext(options);

            _context.Database.EnsureCreated();
            
            var customers = new[]
            {
                    new Customer { Id = 1, firstName = "Steve", lastName = "Rogers", dateOfBirth = "02/02/1985"},
                    new Customer { Id = 2, firstName = "Tony", lastName = "Stark", dateOfBirth = "03/03/1980" },
                    new Customer { Id = 3, firstName = "Bruce", lastName = "Banner", dateOfBirth = "04/04/1960" },
                    new Customer { Id = 4, firstName = "Peter", lastName = "Parker", dateOfBirth = "05/05/2002" }
            };

            _context.Customers.AddRange(customers);
            _context.SaveChanges();

            var controller = new CustomersController(_context);

            var query = new CustomersController(_context);

        }

        [Fact]
        public void Get_CheckCountOfGet_ReturnCount()
        {
            var controller = new CustomersController(_context);
            var result = controller.Get(sort: "asc").CountAsync();
            Assert.Equal(4, result.Result);
        }

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

        [Theory]
        [InlineData("Ste", "Rog", "Steven")]
        public void SearchByName_CheckFirstName_ReturnFirstName(string fName, string lName, string firstName)
        {
            var controller = new CustomersController(_context);
            var result = controller.SearchByName(fName, lName);

            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as alintaApi.Models.Customer;
            var actual = model.firstName;
            Assert.Equal(firstName, actual);
        }

        [Fact]
        public void Post_CheckReturnType_ReturnIActionResult()
        {
            var controller = new CustomersController(_context);
            var result = controller.Post(new Customer { Id = 7, firstName = "Natasha", lastName = "Romanov", dateOfBirth = "07/07/1985" });
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Post_CheckCountAfterPost_ReturnCount()
        {
            var controller = new CustomersController(_context);

            controller.Post(new Customer { Id = 7, firstName = "Natasha", lastName = "Romanov", dateOfBirth = "07/07/1985" });
            var result = controller.Get(sort: "asc").CountAsync();
            Assert.Equal(5, result.Result);
        }

        [Fact]
        public void Put_CheckLastNameAfterChange_ReturnLastName()
        {
            var controller = new CustomersController(_context);
            var result = controller.Put(1, new Customer { Id = 7, firstName = "Steve", lastName = "Jackson", dateOfBirth = "02/02/1985" });

            var okObjectResult = result as OkObjectResult;
            var model = okObjectResult.Value as alintaApi.Models.Customer;
            var actual = model.lastName;
            Assert.Equal("Jackson", actual);
        }

        [Fact]
        public void Delete_CheckCountAfterDelete_ReturnCount()
        {
            var controller = new CustomersController(_context);

            controller.Delete(1);
            var result = controller.Get(sort: "asc").CountAsync();
            Assert.Equal(3, result.Result);
        }

        [Fact]
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
