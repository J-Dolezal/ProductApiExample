using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Product> GetAll(PagingParam? paging = null)
        {
            IQueryable<Product> query = _context.Products;

            if (paging != null)
            {
                if (paging.Value.Limit == 0) throw new ArgumentException($"The {nameof(paging.Value.Limit)} parameter of the {nameof(paging)} parameter must be greater than 0.", nameof(paging.Value.Limit));

                query = query
                    .OrderBy(p => p.Id)
                    .Skip((int)paging.Value.Offset)
                    .Take((int)paging.Value.Limit);
            }
            return query;
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
