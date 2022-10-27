using System;
using Xunit;
using Moq;
using Amazon.S3;
using System.Threading;
using Amazon.S3.Model;
using System.Threading.Tasks;
using Catalog.Lambda.API.Controllers;
using Catalog.Lambda.API.Services;
using System.Collections.Generic;
using Catalog.Lambda.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Lambda.API.Test
{
    public class UnitTest1
    {
        //private Mock<IAmazonS3> mockAmazonClient;
        private Mock<IS3Service> mockS3Service;
        protected CatalogController catalogController;


        [Fact]
        public void GetCatalogByIdTest()
        {
            TestSetup();

            mockS3Service.Setup(m => m.GetProductFromS3("1"))
                .Returns(Task.FromResult(new Models.Product
                {
                    Id = "5",
                    Category = "Test Category"
                }));

            var result = catalogController.GetCatalogById("1");
            Assert.NotNull(result);
        }



        private void TestSetup()
        {
            //mockAmazonClient = new Mock<IAmazonS3>();
            mockS3Service = new Mock<IS3Service>();
            catalogController = new CatalogController(mockS3Service.Object);
        }

        [Fact]
        public async void GetAllCatalogTest_Return_OkResult()
        {
            IEnumerable<Product> products = GetList(); 


            TestSetup();
            mockS3Service.Setup(m => m.GetAllProductsFromS3()).Returns(
                Task.FromResult(products));
            var data = await catalogController.GetAllCatalogDetails("1");
            Assert.NotNull(data.Result);
        }

        [Fact]
        public async void GetAllCatalogTest_Return_NotFoundResult()
        {
            IEnumerable<Product> products = GetList(); 

            TestSetup();
            mockS3Service.Setup(m => m.GetAllProductsFromS3()).Returns(
                Task.FromResult(products));
            var data = await catalogController.GetAllCatalogDetails("1");
            Assert.Null(data.Result); 
        }

        [Fact]
        public async void GetAllCatalogTest_Return_BadRequestResult()
        {
            IEnumerable<Product> products = GetList(); 

            TestSetup();
            mockS3Service.Setup(m => m.GetAllProductsFromS3()).Returns(
                Task.FromResult(products));
            var data = await catalogController.GetAllCatalogDetails("");
            Assert.IsType<BadRequestResult>(data.Result);
            
        }

        private IEnumerable<Product> GetList()
        {
            List<Product> products = new List<Product>();
            products.Add(new Product
            {
                Id = "1",
                Category = "Test Category"
            });
            return products;
        }
    }

}
