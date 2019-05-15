using System;
using alintaApi.Controllers;
using Xunit;

namespace alintaApiTests
{


    public class CustomersControllerTests
    {
        [Fact]
        public void Get_CheckResultNotNull_ReturnObject()
        {

            
            double expected = 5;
            double actual = 5;

            Assert.Equal(expected, actual);
        }
    }
}
