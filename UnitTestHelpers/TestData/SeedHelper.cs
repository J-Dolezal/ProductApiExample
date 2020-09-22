using System;
using System.Collections.Generic;
using ProductApiExample.DataLayer.Entities;

namespace ProductApiExample.UnitTestHelpers
{
    public static class SeedHelper
    {
        public const int ProductSeedCount = 5;

        public static IEnumerable<Product> CreateTestProducts()
        {
            var id = 0;
            while (id++ < ProductSeedCount) yield return new Product(id, $"name {id}", new Uri($"http://domain.domain/{id}"), id * 10, $"Description {id}");
        }
    }
}
