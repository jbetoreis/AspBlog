using System.ComponentModel.DataAnnotations;

namespace AspBlog.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Informe o e-mail")]
    [EmailAddress(ErrorMessage = "O e-mail informado é inválido")]
    public required string Email { get; set; }
    [Required(ErrorMessage = "Informe a senha")]
    public required string Password { get; set; }
}