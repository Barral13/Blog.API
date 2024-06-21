using System.Text.RegularExpressions;
using Blog.API.Data;
using Blog.API.Extensions;
using Blog.API.Models;
using Blog.API.Services;
using Blog.API.ViewModels;
using Blog.API.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.API.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
   [HttpPost("v1/accounts/")]
   public async Task<IActionResult> Post(
      [FromBody] RegisterViewModel model,
      [FromServices] EmailService emailService,
      [FromServices] BlogDataContext context)
   {
      if (!ModelState.IsValid)
         return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

      var user = new User
      {
         Name = model.Name,
         Email = model.Email,
         Slug = model.Email.Replace("@", "-").Replace(".", "-"),
      };

      var password = PasswordGenerator.Generate(25);
      user.PasswordHash = PasswordHasher.Hash(password);

      try
      {
         await context.Users.AddAsync(user);
         await context.SaveChangesAsync();

         emailService.Send(
            user.Name,
            user.Email,
            subject: "Bem vindo ao Blog",
            body: $"Sua senhá é <strong>{password}</strong>");
         return Ok(new ResultViewModel<dynamic>(new
         {
            User = user.Email
         }));
      }
      catch (DbUpdateException)
      {
         return StatusCode(400, new ResultViewModel<string>("05X99 - E-mail já cadastrado em nossa base de dados"));
      }
      catch
      {
         return StatusCode(500, new ResultViewModel<string>("05X07 - Falha interna no servidor"));
      }
   }

   [HttpPost("v1/accounts/login")]
   public async Task<IActionResult> Login(
      [FromBody] LoginViewModel model,
      [FromServices] BlogDataContext context,
      [FromServices] TokenService tokenService)
   {
      if (!ModelState.IsValid)
         return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

      var user = await context
         .Users
         .AsNoTracking()
         .Include(x => x.Roles)
         .FirstOrDefaultAsync(x => x.Email == model.Email);

      if (user == null)
         return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

      if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
         return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

      try
      {
         var token = tokenService.GenerateToken(user);
         return Ok(new ResultViewModel<string>(token, null));
      }
      catch
      {
         return StatusCode(500, new ResultViewModel<string>("05X08 - Falha interna no servidor"));
      }
   }

   [Authorize]
   [HttpPost("v1/accounts/upload-image")]
   public async Task<IActionResult> UploadImage(
      [FromBody] UploadImageViewModel model,
      [FromServices] BlogDataContext context)
   {
      var fileName = $"{Guid.NewGuid().ToString()}.jpg";
      var data = new Regex(@"^data:image\/[a-z]+;base64,")
         .Replace(model.Base64Image, "");
      var bytes = Convert.FromBase64String(data);

      try
      {
         await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
      }
      catch
      {
         return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servodor"));
      }

      var user = await context
         .Users
         .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

      if (user == null)
         return NotFound(new ResultViewModel<Category>("Usuário não encontrado"));

      user.Image = $"https://localhost:0000/images{fileName}";
      try
      {
         context.Users.Update(user);
         await context.SaveChangesAsync();
      }
      catch
      {
         return StatusCode(500, new ResultViewModel<string>("05X05 - Falha interna no servodor"));
      }

      return Ok(new ResultViewModel<string>("Imagem alterada com sucesso!"));
   }
}