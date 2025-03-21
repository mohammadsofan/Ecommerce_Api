using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mshop.Api.Data.models;
using Mshop.Api.Services;

namespace Mshop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            return Ok(categoryService.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            return Ok(categoryService.Get(c => c.Id == id));
        }
        [HttpPost("")]
        public IActionResult Create([FromBody] Category category)
        {
            var categoryInDB = categoryService.Add(category);
            return CreatedAtAction(nameof(GetById), new { id = categoryInDB.Id }, categoryInDB);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var result = categoryService.Delete(id);
            if(result == false)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] Guid id,[FromBody] Category category)
        {
            var result = categoryService.Edit(id, category);
            if (result == false) return NotFound();
            return NoContent();
        }
    }
}
