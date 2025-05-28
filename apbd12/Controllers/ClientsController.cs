using apbd12.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient, CancellationToken token)
    {
        if (!await _clientService.ClientExistsByIdAsync(token, idClient))
        {
            return NotFound("Client not found");
        }

        if (await _clientService.HasTripsAsync(token, idClient))
        {
            return BadRequest("Client has assigned trips");
        }

        await _clientService.DeleteClientAsync(idClient, token);
        return Ok();
    }
}