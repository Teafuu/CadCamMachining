using CadCamMachining.Server.Data;
using CadCamMachining.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadCamMachining.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class MaterialController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MaterialController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Material
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Material>>> GetMaterials()
    {
        return await _context.Materials.ToListAsync();
    }

    // GET: api/Material/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Material>> GetMaterial(Guid id)
    {
        var material = await _context.Materials.FindAsync(id);

        if (material == null)
        {
            return NotFound();
        }

        return material;
    }

    // PUT: api/Material/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMaterial(Guid id, Material material)
    {
        if (id != material.Id)
        {
            return BadRequest();
        }

        _context.Entry(material).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MaterialExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Material
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Material>> PostMaterial(Material material)
    {
        _context.Materials.Add(material);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMaterial", new { id = material.Id }, material);
    }

    // DELETE: api/Material/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterial(Guid id)
    {
        var material = await _context.Materials.FindAsync(id);
        if (material == null)
        {
            return NotFound();
        }

        _context.Materials.Remove(material);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MaterialExists(Guid id)
    {
        return _context.Materials.Any(e => e.Id == id);
    }
}