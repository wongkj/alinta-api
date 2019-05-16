using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alintaApi.Models;

namespace alintaApi.Data
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CustomersDbContext(serviceProvider.GetRequiredService<DbContextOptions<CustomersDbContext>>()))
            {
                if (context.Customers.Any())
                {
                    return;
                }

                context.Customers.AddRange
                (
                    new Customer { Id = 1, firstName = "Steve", lastName = "Rogers", dateOfBirth = "02/02/1985" },
                    new Customer { Id = 2, firstName = "Tony", lastName = "Stark", dateOfBirth = "03/03/1980" },
                    new Customer { Id = 3, firstName = "Bruce", lastName = "Banner", dateOfBirth = "04/04/1960" },
                    new Customer { Id = 4, firstName = "Peter", lastName = "Parker", dateOfBirth = "05/05/2002" }
                );

                context.SaveChanges();
            }
            
        }
    }
}
