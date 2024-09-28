using System.ComponentModel.DataAnnotations;

namespace AspBlog.ViewModels.Categories
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O campo nome deve conter ao menos 3 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O campo slug é obrigatório")]
        public string Slug { get; set; }
    }
}