using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ProductApiExample.Api.Controllers;
using ProductApiExample.ServiceLayer.Services;
using ProductApiExample.UnitTestHelpers;
using ProductDto = ProductApiExample.Api.Dto.v1_0.Product;
using ProductEntity = ProductApiExample.DataLayer.Entities.Product;

namespace ProductApiExample.Api.Test
{
    public class ProductControllerTest
    {
        [Test]
        public void Get_ReturnsAll()
        {
            var entities = SeedHelper.CreateTestProducts().ToList();
            
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(service => service.GetAll(null))
                .Returns(entities);

            var tested = new ProductsController(CreateLoggerMock(), productServiceMock.Object, Utils.MappingHelper.CreateMapper());

            var result = tested.Get().ToList();

            Assert.AreEqual(entities.Count, result.Count);

            // count of matching input entities and output DTOs 
            var countOfMatching = entities
                .Join(result, e => e.Id, dto => dto.Id, (e, dto) => Equals(e, dto))
                .Where(match => match)
                .Count();

            Assert.AreEqual(entities.Count, countOfMatching, "Only {0} DTOs obtained from controller and entities passed to controller match, but total count is {1}.", countOfMatching, entities.Count);
        }

        [Test]
        public async Task GetInt_ProductFound_Returns200AndProduct()
        {
            var productEntity = new ProductEntity(5, "xxx", new Uri("https://domain.countrydomain"), 45, "skdfasdkfjal");
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(service => service.Get(1))
                .Returns(Task.FromResult<ProductEntity?>(productEntity));

            var tested = new ProductsController(CreateLoggerMock(), productServiceMock.Object, ProductApiExample.Api.Utils.MappingHelper.CreateMapper());

            var result = (await tested.Get(1)).Result;

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsTrue(Equals(productEntity, (ProductDto)((OkObjectResult)result).Value));
        }

        [Test]
        public async Task GetInt_ProductNotFound_Returns404()
        {
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(service => service.Get(1))
                .Returns(Task.FromResult<ProductEntity?>(null));

            var tested = new ProductsController(CreateLoggerMock(), productServiceMock.Object, CreateMapperMock());

            var result = await tested.Get(1);

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task SetDescription_Success_Returns204()
        {
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(service => service.SetDescription(1, null))
                .Returns(Task.FromResult(UpdateResult.Success));

            var tested = new ProductsController(CreateLoggerMock(), productServiceMock.Object, CreateMapperMock());

            var result = await tested.SetDescription(1, null);
            Assert.IsInstanceOf<NoContentResult>(result);
        }
        
        [Test]
        public async Task SetDescription_NotFound_Returns404()
        {
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(service => service.SetDescription(1, null))
                .Returns(Task.FromResult(UpdateResult.NotFound));

            var tested = new ProductsController(CreateLoggerMock(), productServiceMock.Object, CreateMapperMock());

            var result = await tested.SetDescription(1, null);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        private static ILogger<ProductsController> CreateLoggerMock() => new Mock<ILogger<ProductsController>>().Object;
        private static IMapper CreateMapperMock() => new Mock<IMapper>().Object;

        private bool Equals(ProductEntity? e, ProductDto? dto)
        {
            if (e == null)
            {
                return dto == null;
            }

            if (dto == null) return false;

            return
                e.Id == dto.Id &&
                e.Price == dto.Price &&
                EqualityComparer<Uri>.Default.Equals(e.ImgUri, dto.ImgUri) &&
                e.Name == dto.Name &&
                e.Description == dto.Description;
        }
    }
}