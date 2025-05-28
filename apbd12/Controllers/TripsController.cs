using apbd12.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips(CancellationToken token, int pageNum = 1, int pageSize = 10)
    {
        var result = await _tripService.GetTripsAsync(pageNum, pageSize, token);
        return Ok(result);
    }
}