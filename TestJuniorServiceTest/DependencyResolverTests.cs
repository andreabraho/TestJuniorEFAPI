using DataLayer.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TestJuniorEFAPI;

namespace TestJuniorServiceTest
{
    [TestClass]
    public class DependencyResolverTests
    {
        private DependencyResolverHelper _serviceProvider;

        public DependencyResolverTests()
        {

            var webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
            _serviceProvider = new DependencyResolverHelper(webHost);
        }

        [TestMethod]
        public void Service_Should_Get_Resolved()
        {

            //Act
            var YourService = _serviceProvider.GetService<IProductRepository>();

            //Assert
            Assert.IsNotNull(YourService);
        }


    }
}
