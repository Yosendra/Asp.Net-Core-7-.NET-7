using CityManager.WebApi.Entities;
using CityManager.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityManager.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CityController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/City
    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetCity()
    {
        if (_context.City == null)
        {
            return NotFound();
        }
        return await _context.City.ToListAsync();
    }

    // GET: api/City/5
    [HttpGet("{id}")]
    public async Task<ActionResult<City>> GetCity(Guid id)
    {
        if (_context.City == null)
        {
            return NotFound();
        }
        var city = await _context.City.FindAsync(id);

        if (city == null)
        {
            return NotFound();
        }

        return city;
    }

    // PUT: api/City/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCity(Guid id, City city)
    {
        if (id != city.Id)
        {
            return BadRequest();
        }

        _context.Entry(city).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CityExists(id))
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

    // POST: api/City
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<City>> PostCity(City city)
    {
        if (_context.City == null)
        {
            return Problem("Entity set 'ApplicationDbContext.City'  is null.");
        }
        _context.City.Add(city);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCity", new { id = city.Id }, city);
    }

    // DELETE: api/City/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(Guid id)
    {
        if (_context.City == null)
        {
            return NotFound();
        }
        var city = await _context.City.FindAsync(id);
        if (city == null)
        {
            return NotFound();
        }

        _context.City.Remove(city);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CityExists(Guid id)
    {
        return (_context.City?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
