using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductApiExample.Api.Dto.v1_0;
using ProductApiExample.ServiceLayer.Services;

namespace ProductApiExample.Api.Controllers.v1_0
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public const int DefaultPageSize = 10;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService, IMapper mapper)
        {
            _logger = logger;
            _productService = productService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        [MapToApiVersion("1.0")]
        public IEnumerable<Product> Get()
        {
            return _productService.GetAll().Select(entity => _mapper.Map<Product>(entity));
        }

        /// <summary>
        /// Gets product by ID
        /// </summary>
        /// <param name="productId">ID of requested product</param>
        [HttpGet("{productId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult<Product>> Get([FromRoute] int productId)
        {
            var p = await _productService.Get(productId);

            if (p == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Product>(p));
        }

        /// <summary>
        /// Updates product description
        /// </summary>
        /// <param name="productId">Product id</param>
        /// <param name="description">Description to set</param>
        [HttpPatch("{productId}/description")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        public async Task<ActionResult> SetDescription([FromRoute] int productId, [FromBody] string? description)
        {
            var operationResult = await _productService.SetDescription(productId, description);
            switch (operationResult)
            {
                case UpdateResult.NotFound:
                    return NotFound();
                case UpdateResult.Success:
                    return NoContent();
                default:
                    // for case of some forgoten implementation when new state in UpdateResult comes up
                    throw new NotImplementedException($"Service returned unexpected result code {operationResult}");
            }
        }
    }
}
