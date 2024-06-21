using Blog.API.Data;
using Blog.API.Extensions;
using Blog.API.Models;
using Blog.API.ViewModels;
using Blog.API.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
   // Get all categories (READ ALL)
   [HttpGet("v1/categories")]
   public async Task<IActionResult> GetAsync(
       [FromServices] BlogDataContext context)
   {
      try
      {
         var categories = await context.Categories.ToListAsync();
         return Ok(new ResultViewModel<List<Category>>(categories));
      }
      catch
      {
         return StatusCode(500, new ResultViewModel<List<Category>>("05XE10 - Falha interna no servidor"));
      }
   }

   // Get Category by Id (READ)
   [HttpGet("v1/categories/{id:int}")]
   public async Task<IActionResult> GetByIdAsync(
       [FromRoute] int id,
       [FromServices] BlogDataContext context)
   {
      try
      {
         var category = await context
             .Categories
             .FirstOrDefaultAsync(x => x.Id == id);

         if (category == null)
            return NotFound(new ResultViewModel<Category>("04XN01 - Conteúdo não encontrado"));

         return Ok(new ResultViewModel<Category>(category));
      }
      catch
      {
         return StatusCode(500, new ResultViewModel<Category>("05XE08 - Falha interna no servidor"));
      }
   }

   // Post category (CREATE)
   [HttpPost("v1/categories")]
   public async Task<IActionResult> PostAsync(
       [FromBody] EditorCategoryViewModel model,
       [FromServices] BlogDataContext context)
   {
      if (!ModelState.IsValid)
         return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

      try
      {
         var category = new Category
         {
            Id = 0,
            Name = model.Name,
            Slug = model.Slug.ToLower(),
         };

         await context.Categories.AddAsync(category);
         await context.SaveChangesAsync();

         return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
      }
      catch (DbUpdateException ex)
      {
         return StatusCode(500, new ResultViewModel<Category>("05XE01 - Não foi possivel incluir a categoria"));
      }
      catch
      {
         return StatusCode(500, new ResultViewModel<Category>("05XE02 - Falha interna no servidor"));
      }
   }

   // Put category (UPDADE)
   [HttpPut("v1/categories/{id:int}")]
   public async Task<IActionResult> PutAsync(
       [FromRoute] int id,
       [FromBody] EditorCategoryViewModel model,
       [FromServices] BlogDataContext context)
   {
      try
      {
         var category = await context
        .Categories
        .FirstOrDefaultAsync(x => x.Id == id);

         if (category == null)
            return NotFound(new ResultViewModel<Category>("04XN02 - Conteúdo não encontrado"));

         category.Name = model.Name;
         category.Slug = model.Slug;

         context.Categories.Update(category);
         await context.SaveChangesAsync();

         return Ok(new ResultViewModel<Category>(category));
      }
      catch (DbUpdateException ex)
      {
         return StatusCode(500, new ResultViewModel<Category>("05XE03 - Não foi possivel alterar a categoria"));
      }
      catch (Exception ex)
      {
         return StatusCode(500, new ResultViewModel<Category>("05XE04 - Falha interna no servidor"));
      }
   }

   // Delete category (DELETE)
   [HttpDelete("v1/categories/{id:int}")]
   public async Task<IActionResult> DeleteAsync(
       [FromRoute] int id,
       [FromServices] BlogDataContext context)
   {
      try
      {
         var category = await context
             .Categories
             .FirstOrDefaultAsync(x => x.Id == id);

         if (category == null)
            return NotFound(new ResultViewModel<Category>("04XN03 - Conteúdo não encontrado"));

         context.Categories.Remove(category);
         await context.SaveChangesAsync();

         return Ok(new ResultViewModel<Category>(category));
      }
      catch (DbUpdateException ex)
      {
         return StatusCode(500, new ResultViewModel<Category>("05XE05 - Não foi possivel remover a categoria"));
      }
      catch (Exception ex)
      {
         return StatusCode(500, new ResultViewModel<Category>("05XE06 - Falha interna no servidor"));
      }
   }
}
