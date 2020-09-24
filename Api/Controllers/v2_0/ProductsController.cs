using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductApiExample.Api.Dto.v1_0;
using ProductApiExample.ServiceLayer.Services;

namespace ProductApiExample.Api.Controllers.v2_0
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
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
            PagingParam? paging = null;

            if (!(offset == null && limit == null))
            {
                // pagination requested
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
            }

            if (ModelState.IsValid)
            {
                return Ok(_productService
                    .GetAll(paging)
                    .Select(entity => _mapper.Map<Product>(entity)));
            }

            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }
}
