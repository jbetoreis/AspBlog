using AspBlog.Data;
using AspBlog.Extensions;
using AspBlog.Models;
using AspBlog.Services;
using AspBlog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace AspBlog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    [HttpPost("v1/accounts/login")]
    public IActionResult Login([FromServices] TokenService tokenService)
    {
        var token = tokenService.GenerateToken(null);
        return Ok(token);
    }

    [HttpPost("v1/accounts")]
    public async Task<IActionResult> PostAsync(
        [FromServices] DataContext ctx,
        [FromBody] RegisterViewModel viewModel
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<dynamic>(ModelState.GetErrors()));
        
        try
        {
            var password = PasswordGenerator.Generate(length: 25, includeSpecialChars: true);
            var user = new User
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Slug = viewModel.Email.Replace("@", "-").Replace(".", "-"),
                PasswordHash = PasswordHasher.Hash(password)
            };
            
            await ctx.Users.AddAsync(user);
            await ctx.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                user.Name,
                user.Email,
                user.PasswordHash
            }));
        }
        catch (DbUpdateException e)
        {
            return StatusCode(400, new ResultViewModel<string>("O e-mail informado já foi cadastrado no sistema"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Houve um erro interno no servidor"));
        }
    }
}