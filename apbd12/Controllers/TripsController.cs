using apbd12.Models.Dtos;
using apbd12.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;
    private readonly IClientService _clientService;

    public TripsController(ITripService tripService, IClientService clientService)
    {
        _tripService = tripService;
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips(CancellationToken token, int pageNum = 1, int pageSize = 10)
    {
        var result = await _tripService.GetTripsAsync(pageNum, pageSize, token);
        return Ok(result);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientForTrip([FromBody] ClientRegistrationDto dto, int idTrip,
        CancellationToken token)
    {
        if (await _clientService.ClientExistsByPeselAsync(dto.Pesel, token))
            return BadRequest($"Client with PESEL - {dto.Pesel} already exists");

        if (await _clientService.ClientRegisteredByPeselAsync(dto.Pesel, idTrip, token))
            return BadRequest($"Client with PESEL - {dto.Pesel} already registered for trip with ID {idTrip}");

        if (!await _tripService.TripExistsAsync(idTrip, token))
            return BadRequest($"Trip with ID {idTrip} does not exists");

        if (await _tripService.TripIsFutureAsync(idTrip, token))
            return BadRequest($"Trip with ID {idTrip} has already passed");

        await _clientService.RegisterClientForTripAsync(dto, idTrip, token);
        return Ok();
    }
}