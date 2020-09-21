﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductApiExample.Api.Dto;
using ProductApiExample.ServiceLayer.Services;

namespace ProductApiExample.Api.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(ILogger<ProductController> logger, IProductService productService, IMapper mapper)
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
        public IEnumerable<Product> Get()
        {
            return _productService.GetAll().Select(entity => _mapper.Map<Product>(entity)).AsEnumerable();
        }    
        
        /// <summary>
        /// Gets product by ID
        /// </summary>
        /// <param name="productId">ID of requested product</param>
        [HttpGet("{productId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
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