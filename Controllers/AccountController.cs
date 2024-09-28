using System.Text.RegularExpressions;
using AspBlog.Data;
using AspBlog.Extensions;
using AspBlog.Models;
using AspBlog.Services;
using AspBlog.ViewModels;
using AspBlog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace AspBlog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginViewModel viewModel,
        [FromServices] DataContext ctx,
        [FromServices] TokenService tokenService
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        try
        {
            var user = await ctx.Users.AsNoTracking().Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == viewModel.Email);

            if (user == null)
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválido"));

            if (!PasswordHasher.Verify(user.PasswordHash, viewModel.Password))
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválido"));

            var token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Houve um erro interno no servidor"));
        }
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
                password
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
    
    [Authorize]
    [HttpPost("v1/accounts/upload-image")]
    public async Task<IActionResult> UploadImage(
        [FromServices] DataContext ctx,
        [FromServices] TokenService tokenService,
        [FromBody] UploadImageViewModel viewModel
    )
    {
        var fileName = $"{Guid.NewGuid().ToString()}.jpg";
        var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(viewModel.Base64Image, "");
        var bytes = Convert.FromBase64String(data);

        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor"));
        }

        var user = await ctx.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
        
        if (user == null)
            return NotFound(new ResultViewModel<Category>("Usuário não encontrado"));

        user.Image = $"https://localhost:7088/images/{fileName}";
        try
        {
            ctx.Users.Update(user);
            await ctx.SaveChangesAsync();
            
            return Ok(new ResultViewModel<string>("Imagem alterada com sucesso!", null));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor"));
        }
    }
}