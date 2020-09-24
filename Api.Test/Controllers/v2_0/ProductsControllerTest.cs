using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ProductApiExample.Api.Controllers.v2_0;
using ProductApiExample.Api.Dto.v1_0;
using ProductApiExample.Api.Test.Helpers;
using ProductApiExample.ServiceLayer.Services;
using ProductApiExample.UnitTestHelpers;

namespace ProductApiExample.Api.Test.Controllers.v2_0
{
    public class ProductsControllerTest
    {
        [Test]
        public void Get_NoPagingRequested_ReturnsAll()
        {
            var entities = SeedHelper.CreateTestProducts().ToList();

            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(service => service.GetAll(null))
                .Returns(entities);

            var tested = new ProductsController(ServiceMockHelper.CreateLoggerMock<ProductsController>(), productServiceMock.Object, Utils.MappingHelper.CreateMapper());

            var actionResult = tested.Get(null, null).Result;

            productServiceMock.VerifyAll();
            Assert.IsInstanceOf<OkObjectResult>(actionResult);

            var resultData = ((IEnumerable<Product>)((OkObjectResult)actionResult).Value).ToList();
            Dto_1_0_Helper.VerifyCollectionMatch(entities, resultData);     
        }

        [TestCase(null, 8U)]
        [TestCase(0U, 0U)]
        public void Get_InvalidPagingParams_Returns400(uint? offset, uint? limit)
        {
            var tested = new ProductsController(ServiceMockHelper.CreateLoggerMock<ProductsController>(), new Mock<IProductService>().Object, ServiceMockHelper.CreateMapperMock());

            var result = tested.Get(offset, limit).Result;
            
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void Get_PagingSet_CorrectDtosReturned()
        {
            const uint offset = 1;
            const uint limit = 2;

            var entities = SeedHelper.CreateTestProducts().ToList();

            var productServiceMock = new Mock<IProductService>();
            productServiceMock
                .Setup(service => service.GetAll(It.Is<PagingParam?>(pg => pg != null && pg.Value.Limit == limit && pg.Value.Offset == offset)))
                .Returns(entities);

            var tested = new ProductsController(ServiceMockHelper.CreateLoggerMock<ProductsController>(), productServiceMock.Object, Utils.MappingHelper.CreateMapper());

            var actionResult = tested.Get(offset, limit).Result;

            productServiceMock.VerifyAll();
              
            Assert.IsInstanceOf<OkObjectResult>(actionResult);

            var resultData = ((IEnumerable<Product>)((OkObjectResult)actionResult).Value).ToList();

            Dto_1_0_Helper.VerifyCollectionMatch(entities, resultData);
        }
    }
}
