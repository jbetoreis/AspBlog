using System.ComponentModel.DataAnnotations;

namespace AspBlog.ViewModels.Accounts;

public class UploadImageViewModel
{
    [Required(ErrorMessage = "Informe a imagem")]
    public string Base64Image { get; set; }
}