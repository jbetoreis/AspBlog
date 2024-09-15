using AspBlog.Data;
using AspBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspBlog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetAsync(
            [FromRoute] int id,
            [FromServices] DataContext ctx
        )
        {
            var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpGet("v1/categories")]
        public async Task<IActionResult> ListAsync([FromServices] DataContext ctx)
        {
            var categories = await ctx.Categories.AsNoTracking().ToListAsync();
            return Ok(categories);
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromBody] Category category,
            [FromServices] DataContext ctx
        )
        {
            await ctx.Categories.AddAsync(category);
            await ctx.SaveChangesAsync();
            return Created($"v1/categories/{category.Id}", category);
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] Category category,
            [FromServices] DataContext ctx
        )
        {
            var targetCategory = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (targetCategory == null)
                return NotFound();
            category.Name = targetCategory.Name;
            category.Slug = targetCategory.Slug;
            ctx.Categories.Update(category);
            await ctx.SaveChangesAsync();
            return Ok(category);
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] DataContext ctx
        )
        {
            var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();
            ctx.Categories.Remove(category);
            await ctx.SaveChangesAsync();
            return Ok(category);
        }
    }
}