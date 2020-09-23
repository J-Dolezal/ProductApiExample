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

namespace ProductApiExample.Api.Controllers
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
        /// Gets all products
        /// </summary>
        /// <param name="offset">Pagination - zero based index of starting record</param>
        /// <param name="limit">Pagination - count of returned records (page size) Default is <see cref="DefaultPageSize"/></param>
        /// <remarks>
        /// When no <paramref name="limit"/> and <paramref name="offset"/> is sent, then pagination is inactive and all records are returned.
        /// 
        /// Otherwise pagination is active. In that case <paramref name="limit"/> must be greater than zero or null. If it is null, it falls back to
        /// default value which is <see cref="DefaultPageSize"/>.
        /// </remarks>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [MapToApiVersion("2.0")]
        public ActionResult<IEnumerable<Product>> Get([FromQuery] uint? offset, [FromQuery] uint? limit)
        {
            if (offset==null && limit == null)
            {
                // no pagination requested
                return Ok(Get());
            }

            // Getting list with pagination

            if (offset == null)
            {
                ModelState.AddModelError(nameof(offset), $"Both, {nameof(offset)} and {nameof(limit)}, must be specified for pagination.");                
            }

            if (limit == null) 
            { 
                limit = DefaultPageSize; 
            }
            else if (limit.Value == 0)
            {
                ModelState.AddModelError(nameof(limit), $"{nameof(limit)} must be greater than 0");
            }

            if (ModelState.IsValid)
            {
                return Ok(_productService
                    .GetAll(new PagingParam
                    {
                        Limit = limit ?? DefaultPageSize,
                        Offset = offset.Value
                    })
                    .Select(entity => _mapper.Map<Product>(entity)));
            }

            return BadRequest(new ValidationProblemDetails(ModelState));
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
