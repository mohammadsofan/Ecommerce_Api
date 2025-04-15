using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mshop.Api.Data;
using Mshop.Api.Data.models;
using Mshop.Api.DTOs.Requests;
using Mshop.Api.DTOs.Responses;
using Mshop.Api.Services;
using System;

namespace Mshop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IProductService productService;

        public ProductsController(ApplicationDbContext context,IProductService productService)
        {
            this.context = context;
            this.productService = productService;
        }
        [HttpGet("")]
        public IActionResult GetAll([FromQuery] string? query, [FromQuery] int page=1, [FromQuery] int limit = 10)
        {
            try {
                var products = productService.GetAll(query, page,limit);
                if(products is null)
                {
                    return NotFound();
                }
                return Ok(new { products = products.Adapt<IEnumerable<ProductResponse>>(),page,skip=(page-1)*limit,count=products.Count() });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            try
            {
                var product = productService.Get(p=>p.Id==id);
                if (product is null)
                {
                    return NotFound();
                }
                return Ok(product.Adapt<ProductResponse>());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("")]
        public IActionResult Create([FromForm] ProductRequest productRequest)
        {
            try
            {
                var product = productService.Add(productRequest.Adapt<Product>(), productRequest.MainImage);
                return CreatedAtAction(nameof(GetById), new { product.Id }, product.Adapt<ProductResponse>());
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                var result = productService.Delete(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] Guid id, [FromForm] ProductUpdateRequest productRequest)
        {
            try
            {
                var result = productService.Edit(id, productRequest.Adapt<Product>(), productRequest.MainImage);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
