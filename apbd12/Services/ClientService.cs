using apbd12.Data;
using Microsoft.EntityFrameworkCore;

namespace apbd12.Services;

public class ClientService : IClientService
{
    private readonly DatabaseContext _context;

    public ClientService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> HasTrips(CancellationToken token, int idClient)
    {
        var result = await _context.ClientTrips.Where(c => c.IdClient == idClient).CountAsync(token);
        return result > 0;
    }

    public async Task<bool> ClientExists(CancellationToken token, int idClient)
    {
        var result = await _context.Clients.Where(c => c.IdClient == idClient).CountAsync(token);
        return result > 0;
    }

    public async Task DeleteClient(CancellationToken token, int idClient)
    {
        var client = await _context.Clients.Where(c => c.IdClient == idClient).FirstOrDefaultAsync(token);
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync(token);
    }
}