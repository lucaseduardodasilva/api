using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("categories")]
public class CategoryController : ControllerBase 
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get(
        [FromServices]DataContext context)
    {
        var categories = await context.Categories.AsNoTracking().ToListAsync();
        return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id,
    [FromServices]DataContext context)
    {
        var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return category;
        
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Post(
        [FromBody]Category model,
        [FromServices]DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

            try
            {
                context.Categories.Add(model);    
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                
                return BadRequest(new { message = "Nao foi possivel criar a categoria" });
            }  
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Category>>> Put(
        int id, 
        [FromBody]Category model,
        [FromServices]DataContext context)
    {
        if(id != model.Id)
            return NotFound(new { message = "Categoria não encontrada" });

        if(!ModelState.IsValid)
            return BadRequest(ModelState);

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                
                return BadRequest(new { message = "Este registro ja foi atualizado" });
            }
            catch (Exception)
            {
                
                return BadRequest(new { message = "Nao foi atualizar a categoria" });
            }
    }

    [HttpDelete]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Delete(
        int id,
        [FromServices]DataContext context
    )
    {
        var category = await context.Categories.FirstAsync(x => x.Id == id);
            if(category == null)
                return NotFound(new { teste = "Categoria nao foi encontrada"});
        try
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(new { mensagemRemocao = "Categoria removida"});
        }
        catch (Exception)
        {
            
            throw;
        }
    }
}