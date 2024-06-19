using Blog.API.Data;
using Blog.API.Extensions;
using Blog.API.Models;
using Blog.API.Services;
using Blog.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
   [HttpPost("v1/accounts/")]
   public async Task<IActionResult> Post(
      [FromBody] RegisterViewModel model,
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

         return Ok(new ResultViewModel<dynamic>(new
         {
            User = user.Email,
            password
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
}

// Adicionar: dotnet add package SecureIdentity