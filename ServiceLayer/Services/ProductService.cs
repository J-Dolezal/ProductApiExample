using System.Collections.Generic;
using System.Threading.Tasks;
using ProductApiExample.DataLayer.Entities;

namespace ProductApiExample.ServiceLayer.Services
{
    internal class ProductService : IProductService
    {
        public ProductService(DataLayer.Context context)
        {
            _context = context;
        }

        private readonly DataLayer.Context _context;

        public Task<Product?> Get(int id)
        {
            return _context.Products.FindAsync(id).AsTask();
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products;
        }

        public async Task<UpdateResult> SetDescription(int id, string? newDescription)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return UpdateResult.NotFound;

            product.Description = newDescription;

            await _context.SaveChangesAsync();

            return UpdateResult.Success;
        }
    }
}
