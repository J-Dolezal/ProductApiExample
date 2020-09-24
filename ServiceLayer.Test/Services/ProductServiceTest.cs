using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProductApiExample.DataLayer.Entities;
using ProductApiExample.ServiceLayer.Services;
using ProductApiExample.UnitTestHelpers;

namespace ProductApiExample.ServiceLayer.Test
{
    public class ProductServiceTest
    {
        [Test]
        public void GetAll_NullPaging_ReturnsAll()
        {
            using var context = CreateContextWithTestProducts();

            var tested = new ProductService(context);
            var result = tested.GetAll();

            Assert.AreEqual(SeedHelper.ProductSeedCount, result.Count());
        }

        [Test]
        public void GetAll_PagingSet_RetursCorrectEntities()
        {
            const uint limit = 2;
            const uint offset = 1;

            using var context = CreateContextWithTestProducts();
            var paging = new PagingParam { Offset = offset, Limit = limit };

            var tested = new ProductService(context);
            var result = tested.GetAll(paging).ToList();

            Assert.AreEqual(limit, result.Count());
            for (int i = 1; i <= limit; i++)
            {
                Assert.IsTrue(result.Any(p => p.Id == offset + i));
            }
        }

        [Test]
        public void GetAll_PagingLimitIsZero_ArgumentExceptionThrown()
        {
            var builder = new DbContextOptionsBuilder<DataLayer.Context>();
            builder.UseInMemoryDatabase(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());

            using var context = new DataLayer.Context(builder.Options);
            var tested = new ProductService(context);

            Assert.Throws<ArgumentException>(() => tested.GetAll(new PagingParam { Limit = 0, Offset = 0 }));
        }

        [Test]
        public async Task Get_Found_ReturnsProduct()
        {
            using var context = CreateContextWithTestProducts();

            var testId = 4;
            var entity = context.Products.Single(p => p.Id == testId);
            var tested = new ProductService(context);

            var result = await tested.Get(testId);
            Assert.AreEqual(entity, result);
        }
        
        [Test]
        public async Task Get_NotFound_ReturnsNull()
        {
            using var context = CreateContextWithTestProducts();

            var tested = new ProductService(context);
            var result = await tested.Get(SeedHelper.ProductSeedCount + 1);

            Assert.IsNull(result);
        }
        
        [Test]
        public async Task SetDescription_NotFound_ReturnsNotFound()
        {
            using var context = CreateContextWithTestProducts();

            const string testDescription = "TestDescription"; 
            var tested = new ProductService(context);
            var result = await tested.SetDescription(SeedHelper.ProductSeedCount + 1, testDescription);

            Assert.AreEqual(UpdateResult.NotFound, result);
            Assert.AreEqual(0, context.Products.Where(p => p.Description == testDescription).Count(), "There was updated products although they should not be.");
        }

        [TestCase((string?)null)]
        [TestCase("new value")]
        public async Task Get_Found_DescriptionSet(string? valueToSet)
        {
            using var context = CreateContextWithTestProducts(Enumerable.Range(1,6).Select(id => new Product(id, "Name", new System.Uri("http://x.y"), 5, "description")));

            var testId = 4;
            var tested = new ProductService(context);
            var result = await tested.SetDescription(testId, valueToSet);
            var changedEntities = context.Products.Where(p => p.Description == valueToSet).ToList();

            Assert.AreEqual(UpdateResult.Success, result);
            Assert.AreEqual(1, changedEntities.Count, "Description was set for unexpected number of entities");
            Assert.AreEqual(testId, changedEntities[0].Id, "Description was set at wrong entity.");
        }

        private DataLayer.Context CreateContextWithTestProducts() => CreateContextWithTestProducts(SeedHelper.CreateTestProducts());

        private DataLayer.Context CreateContextWithTestProducts(IEnumerable<Product> testData)
        {
            var builder = new DbContextOptionsBuilder<DataLayer.Context>();
            builder.UseInMemoryDatabase(System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            var options = builder.Options;

            using (var context = new DataLayer.Context(options))
            {
                context.Products.RemoveRange(context.Products);
                context.Products.AddRange(testData);
                context.SaveChanges();
            }

            return new DataLayer.Context(options);
        }
    }
}