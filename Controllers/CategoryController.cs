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
            try
            {
                var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound();
                return Ok(category);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Houve um erro interno no servidor");
            }
        }

        [HttpGet("v1/categories")]
        public async Task<IActionResult> ListAsync([FromServices] DataContext ctx)
        {
            try
            {
                var categories = await ctx.Categories.AsNoTracking().ToListAsync();
                return Ok(categories);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Houve um erro interno no servidor");
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromBody] Category category,
            [FromServices] DataContext ctx
        )
        {
            try
            {
                await ctx.Categories.AddAsync(category);
                await ctx.SaveChangesAsync();
                return Created($"v1/categories/{category.Id}", category);
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, "Houve um erro ao tentar salvar os dados no banco");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Houve um erro interno no servidor");
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] Category category,
            [FromServices] DataContext ctx
        )
        {
            try
            {
                var targetCategory = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (targetCategory == null)
                    return NotFound();
                targetCategory.Name = category.Name;
                targetCategory.Slug = category.Slug;
                ctx.Categories.Update(targetCategory);
                await ctx.SaveChangesAsync();
                return Ok(targetCategory);
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, "Houve um erro ao tentar salvar os dados no banco");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Houve um erro interno no servidor");
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] DataContext ctx
        )
        {
            try
            {
                var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound();
                ctx.Categories.Remove(category);
                await ctx.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, "Houve um erro ao tentar apagar os dados do banco");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Houve um erro interno no servidor");
            }
        }
    }
}