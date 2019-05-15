using System;
using alintaApi.Controllers;
using alintaApi.Models;
using alintaApi.Data;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Autofac.Extras.Moq;

namespace alintaApiTests
{
    public interface IDbContext : IDisposable
    {
        
    }

    public class CustomersControllerTests
    {
        private CustomersController controller;

        [Fact]
        public void ShouldReturnAllCustomers()
        {
            var options = new DbContextOptionsBuilder<CustomersDbContext>()
                .UseInMemoryDatabase(databaseName: "CustomersDB")
                .Options;
            var context = new CustomersDbContext(options);
            Seed(context);
            var query = new CustomersController(context);
            var result = query.Get(sort: "asc");
            // Assert.Equal(5, result.Count)
        }

        private void Seed(CustomersDbContext context)
        {
            var customers = new[]
            {
                new Customer { Id = 1, firstName = "Bruce", lastName = "Banner", dateOfBirth = "01/01/2001" },
                new Customer { Id = 2, firstName = "Peter", lastName = "Parker", dateOfBirth = "02/02/1980" },
                new Customer { Id = 3, firstName = "Steve", lastName = "Rogers", dateOfBirth = "03/03/1975" },
                new Customer { Id = 4, firstName = "Tony", lastName = "Stark", dateOfBirth = "04/04/1970" },
                new Customer { Id = 5, firstName = "Steven", lastName = "Strange", dateOfBirth = "05/05/1965" }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
