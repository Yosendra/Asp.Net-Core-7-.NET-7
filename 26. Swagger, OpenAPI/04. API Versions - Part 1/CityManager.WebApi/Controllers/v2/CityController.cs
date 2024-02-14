using CityManager.WebApi.Entities;
using CityManager.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityManager.WebApi.Controllers.v2;

[ApiVersion("2.0")]                                     // notice this
public class CityController : CustomControllerBase
{
    private readonly ApplicationDbContext _context;

    public CityController(ApplicationDbContext context) => _context = context;


    /// <summary>
    /// To get list of cities, contains id and city's name from 'cities' table
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    //[Produces("application/xml")]
    public async Task<ActionResult<IEnumerable<string?>>> GetCity()
    {
        if (_context.City == null)
            return NotFound();

        return await _context.City.Select(c => c.Name).ToListAsync();
    }
}
