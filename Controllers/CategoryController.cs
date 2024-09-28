using AspBlog.Attributes;
using AspBlog.Data;
using AspBlog.Extensions;
using AspBlog.Models;
using AspBlog.ViewModels;
using AspBlog.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspBlog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories/{id:int}")]
        [ApiKey]
        public async Task<IActionResult> GetAsync(
            [FromRoute] int id,
            [FromServices] DataContext ctx
        )
        {
            try
            {
                var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Registro não encontrado"));
                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Houve um erro interno no servidor"));
            }
        }
        
        [HttpGet("v1/categories")]
        [Authorize]
        public async Task<IActionResult> ListAsync([FromServices] DataContext ctx)
        {
            try
            {
                var categories = await ctx.Categories.AsNoTracking().ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("Houve um erro interno no servidor"));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditorCategoryViewModel viewModel,
            [FromServices] DataContext ctx
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category
                {
                    Name = viewModel.Name,
                    Slug = viewModel.Slug
                };
                await ctx.Categories.AddAsync(category);
                await ctx.SaveChangesAsync();
                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new ResultViewModel<Category>("Houve um erro ao tentar salvar os dados no banco"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("Houve um erro interno no servidor"));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel viewModel,
            [FromServices] DataContext ctx
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Registro não encontrado"));
                category.Name = viewModel.Name;
                category.Slug = viewModel.Slug;
                ctx.Categories.Update(category);
                await ctx.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new ResultViewModel<Category>("Houve um erro ao tentar salvar os dados no banco"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Houve um erro interno no servidor"));
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
                    return NotFound(new ResultViewModel<Category>("Registro não encontrado"));
                ctx.Categories.Remove(category);
                await ctx.SaveChangesAsync();
                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new ResultViewModel<Category>("Houve um erro ao tentar apagar os dados do banco"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Houve um erro interno no servidor"));
            }
        }
    }
}