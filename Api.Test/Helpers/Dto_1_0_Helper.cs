using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ProductDto = ProductApiExample.Api.Dto.v1_0.Product;
using ProductEntity = ProductApiExample.DataLayer.Entities.Product;

namespace ProductApiExample.Api.Test.Helpers
{
    /// <summary>
    /// Checks whether DTO collection corresponds with entity collection
    /// </summary>
    /// <remarks>
    /// Uses Assers to verify whether both collection have same size, and each item in one collection has equal item in the seccond 
    /// (each entity must have its DTO projection in second collection and vice versa)
    /// </remarks>
    internal static class Dto_1_0_Helper
    {
        public static void VerifyCollectionMatch(ICollection<ProductEntity> entities, ICollection<ProductDto> dtos)
        {
            Assert.AreEqual(entities.Count, dtos.Count);

            // count of matching input entities and output DTOs 
            var countOfMatching = entities
                .Join(dtos, e => e.Id, dto => dto.Id, (e, dto) => dto.Equals(e))
                .Where(match => match)
                .Count();

            Assert.AreEqual(entities.Count, countOfMatching, "Only {0} DTOs obtained from controller and entities passed to controller match, but total count is {1}.", countOfMatching, entities.Count);
        }
    }
}
