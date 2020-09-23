using System.Collections.Generic;
using System.Threading.Tasks;
using ProductApiExample.DataLayer.Entities;

namespace ProductApiExample.ServiceLayer.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Gets all products
        /// </summary>
        IEnumerable<Product> GetAll(PagingParam? paging = null);
        
        /// <summary>
        /// Returns product with specified ID
        /// </summary>
        /// <param name="id">ID of product</param>
        /// <returns>Product instance or null if none exist with given ID</returns>
        Task<Product?> Get(int id);

        /// <summary>
        /// Sets products description
        /// </summary>
        /// <param name="id">ID of product</param>
        /// <param name="newDescription">Description to be set</param>
        /// <returns>
        /// <see cref="UpdateResult.NotFound"/> if product with given ID does not exist
        /// <see cref="UpdateResult.Success"/> if description was successfuly changed
        /// </returns>
        Task<UpdateResult> SetDescription(int id, string? newDescription);        
    }
}
