using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ProductApiExample.Api.Controllers.v1_0;
using ProductApiExample.Api.Test.Helpers;
using ProductApiExample.ServiceLayer.Services;
using ProductApiExample.UnitTestHelpers;
using ProductDto = ProductApiExample.Api.Dto.v1_0.Product;
using ProductEntity = ProductApiExample.DataLayer.Entities.Product;

namespace ProductApiExample.Api.Test.Controllers.v1_0
{
    public class ProductsControllerTest
    {
        [Test]
        public void Get_ReturnsAll()
        {
            var entities = SeedHelper.CreateTestProducts().ToList();

            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(service => service.GetAll(null))
                .Returns(entities);

            var tested = new ProductsController(ServiceMockHelper.CreateLoggerMock<ProductsController>(), productServiceMock.Object, Utils.MappingHelper.CreateMapper());

            var result = tested.Get().ToList();

            productServiceMock.VerifyAll();
            Assert.AreEqual(entities.Count, result.Count);

            Dto_1_0_Helper.VerifyCollectionMatch(entities, result);
        }

        [Test]
        public async Task GetInt_ProductFound_Returns200AndProduct()
        {
            var productEntity = new ProductEntity(5, "xxx", new Uri("https://domain.countrydomain"), 45, "skdfasdkfjal");
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(service => service.Get(1))
                .Returns(Task.FromResult<ProductEntity?>(productEntity));

            var tested = new ProductsController(ServiceMockHelper.CreateLoggerMock<ProductsController>(), productServiceMock.Object, Utils.MappingHelper.CreateMapper());

            var result = (await tested.Get(1)).Result;

            productServiceMock.VerifyAll();
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsTrue(((ProductDto)((OkObjectResult)result).Value).Equals(productEntity));
        }

        [Test]
        public async Task GetInt_ProductNotFound_Returns404()
        {
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(service => service.Get(1))
                .Returns(Task.FromResult<ProductEntity?>(null));

            var tested = new ProductsController(ServiceMockHelper.CreateLoggerMock<ProductsController>(), productServiceMock.Object, ServiceMockHelper.CreateMapperMock());

            var result = await tested.Get(1);

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task SetDescription_Success_Returns204()
        {
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(service => service.SetDescription(1, null))
                .Returns(Task.FromResult(UpdateResult.Success));

            var tested = new ProductsController(ServiceMockHelper.CreateLoggerMock<ProductsController>(), productServiceMock.Object, ServiceMockHelper.CreateMapperMock());

            var result = await tested.SetDescription(1, null);

            productServiceMock.VerifyAll();
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task SetDescription_NotFound_Returns404()
        {
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(service => service.SetDescription(1, null))
                .Returns(Task.FromResult(UpdateResult.NotFound));

            var tested = new ProductsController(ServiceMockHelper.CreateLoggerMock<ProductsController>(), productServiceMock.Object, ServiceMockHelper.CreateMapperMock());

            var result = await tested.SetDescription(1, null);

            productServiceMock.VerifyAll();
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}