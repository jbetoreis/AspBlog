using System.ComponentModel.DataAnnotations;

namespace AspBlog.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Informe o nome")]
    public required string Name { get; set; }
    
    [Required(ErrorMessage = "Informe o e-mail")]
    [EmailAddress(ErrorMessage = "O e-mail informado é inválido")]
    public required  string Email { get; set; }
}