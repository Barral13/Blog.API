using Blog.API.Data;
using Blog.API.Models;
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
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "05XE10 - Falha interna no servidor");
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
                return NotFound();

            return Ok(category);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "05XE08 - Falha interna no servidor");
        }
    }

    // Post category (CREATE)
    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
        [FromBody] Category model,
        [FromServices] BlogDataContext context)
    {
        try
        {
            await context.Categories.AddAsync(model);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{model.Id}", model);
        }
        catch(DbUpdateException ex)
        {
            return StatusCode(500, "05XE01 - Não foi possivel incluir a categoria");
        }
        catch (Exception ex) 
        {
            return StatusCode(500, "05XE02 - Falha interna no servidor");
        }
    }

    // Put category (UPDADE)
    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromRoute] int id,
        [FromBody] Category model,
        [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context
           .Categories
           .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound();

            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(model);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, "05XE03 - Não foi possivel alterar a categoria");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "05XE04 - Falha interna no servidor");
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
                return NotFound();

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return Ok(category);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, "05XE05 - Não foi possivel remover a categoria");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "05XE06 - Falha interna no servidor");
        }
    }
}
