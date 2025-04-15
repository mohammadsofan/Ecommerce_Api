using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mshop.Api.Data.models;
using Mshop.Api.DTOs.Request;
using Mshop.Api.DTOs.Requests;
using Mshop.Api.DTOs.ResponseDTOs;
using Mshop.Api.DTOs.Responses;
using Mshop.Api.Services;

namespace Mshop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService brandService;

        public BrandsController(IBrandService brandService)
        {
            this.brandService = brandService;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(brandService.GetAll().Adapt<IEnumerable<BrandResponse>>());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            try
            {
                return Ok(brandService.Get(c => c.Id == id).Adapt<BrandResponse>());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("")]
        public IActionResult Create([FromBody] BrandRequest brandRequest)
        {
            try
            {
                var brandInDB = brandService.Add(brandRequest.Adapt<Brand>());
                return CreatedAtAction(nameof(GetById), new { id = brandInDB.Id }, brandInDB);
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
                var result = brandService.Delete(id);
                if (result == false)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] BrandRequest brandRequest)
        {
            try
            {
                var result = brandService.Edit(id, brandRequest.Adapt<Brand>());
                if (result == false) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
