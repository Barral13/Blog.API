using System.ComponentModel.DataAnnotations;

namespace Blog.API.ViewModels.Accounts;
public class LoginViewModel
{
  
   [Required(ErrorMessage = "Informe seu E-mail")]
   [EmailAddress(ErrorMessage = "E-mail inválido")]
   public string Email { get; set; }

   [Required(ErrorMessage = "Informe sua senha")]
   public string Password { get; set; }
}