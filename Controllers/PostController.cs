using AspBlog.Data;
using AspBlog.Models;
using AspBlog.ViewModels;
using AspBlog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspBlog.Controllers;

[ApiController]
public class PostController : ControllerBase
{
    [HttpGet("v1/posts")]
    public async Task<IActionResult> ListAsync(
        [FromServices] DataContext ctx,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10
    )
    {
        try
        {
            var totalPosts = await ctx.Posts.AsNoTracking().CountAsync();
            var posts = await ctx.Posts.AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Select(x => new ListPostsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    LastUpdateDate = x.LastUpdateDate,
                    Category = x.Category.Name,
                    Author = x.Author.Name
                })
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
            return Ok(new ResultViewModel<dynamic>(new
            {
                total = totalPosts,
                offset,
                limit,
                posts
            }));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Houve um erro interno no servidor"));
        }
    }
}