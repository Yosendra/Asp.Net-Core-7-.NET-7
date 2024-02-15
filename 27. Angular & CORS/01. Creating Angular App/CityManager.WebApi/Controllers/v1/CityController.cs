using CityManager.WebApi.Entities;
using CityManager.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityManager.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class CityController : CustomControllerBase
{
    private readonly ApplicationDbContext _context;

    public CityController(ApplicationDbContext context) => _context = context;


    /// <summary>
    /// To get list of cities, contains id and city's name from 'cities' table
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetCity()
    {
        if (_context.City == null)
            return NotFound();

        return await _context.City.ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<City>> GetCity(Guid id)
    {
        if (_context.City == null)
            return NotFound();

        var city = await _context.City.FindAsync(id);
        if (city == null)
            return Problem(detail: "Invalid Id", statusCode: 400, title: "City Search");

        return city;
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutCity(Guid id, City city)
    {
        if (id != city.Id)
            return BadRequest();

        _context.Entry(city).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CityExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }


    [HttpPost]
    public async Task<ActionResult<City>> PostCity(City city)
    {
        if (_context.City == null)
            return Problem("Entity set 'ApplicationDbContext.City' is null.");

        _context.City.Add(city);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCity", new { id = city.Id }, city);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(Guid id)
    {
        if (_context.City == null)
            return NotFound();

        var city = await _context.City.FindAsync(id);
        if (city == null)
            return NotFound();

        _context.City.Remove(city);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    private bool CityExists(Guid id)
    {
        return (_context.City?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
