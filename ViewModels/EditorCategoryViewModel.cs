using System.ComponentModel.DataAnnotations;

namespace Blog.API.ViewModels;

public class EditorCategoryViewModel
{
   [Required(ErrorMessage = "O nome é obrigatório")]
   [StringLength(40, MinimumLength = 3, ErrorMessage = "O campo nome deve conter entre 3 e 40 caracteres")]
   public string Name { get; set; } = string.Empty;

   [Required(ErrorMessage = "O slug é obrigatório")]
   public string Slug { get; set; } = string.Empty;
}