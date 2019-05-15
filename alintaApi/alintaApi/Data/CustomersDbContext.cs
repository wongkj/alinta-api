using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using alintaApi.Models;

namespace alintaApi.Data
{
    public class CustomersDbContext : DbContext
    { 
        public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData
            (
                new Customer
                {
                    Id = 1,
                    firstName = "Steve",
                    lastName = "Rogers",
                    dateOfBirth = "05/05/1910"
                },
                new Customer
                {
                    Id = 2,
                    firstName = "Tony",
                    lastName = "Stark",
                    dateOfBirth = "06/06/1960"
                },
                new Customer
                {
                    Id = 3,
                    firstName = "Peter",
                    lastName = "Parker",
                    dateOfBirth = "02/02/2002"
                },
                new Customer
                {
                    Id = 4,
                    firstName = "Steven",
                    lastName = "Strange",
                    dateOfBirth = "04/04/1975"
                },
                new Customer
                {
                    Id = 5,
                    firstName = "Natasha",
                    lastName = "Romanov",
                    dateOfBirth = "01/01/1985"
                },
                new Customer
                {
                    Id = 6,
                    firstName = "Bruce",
                    lastName = "Banner",
                    dateOfBirth = "07/07/1970"
                }
            );
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
