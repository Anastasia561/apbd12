using apbd12.Data;
using apbd12.Models;
using apbd12.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace apbd12.Services;

public class ClientService : IClientService
{
    private readonly DatabaseContext _context;

    public ClientService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> HasTripsAsync(CancellationToken token, int idClient)
    {
        var result = await _context.ClientTrips.Where(c => c.IdClient == idClient).CountAsync(token);
        return result > 0;
    }

    public async Task<bool> ClientExistsByIdAsync(CancellationToken token, int idClient)
    {
        var result = await _context.Clients.Where(c => c.IdClient == idClient).CountAsync(token);
        return result > 0;
    }

    public async Task<bool> ClientExistsByPeselAsync(string pesel, CancellationToken token)
    {
        var result = await _context.Clients.Where(c => c.Pesel == pesel).CountAsync(token);
        return result > 0;
    }

    public async Task DeleteClientAsync(int idClient, CancellationToken token)
    {
        var client = await _context.Clients.Where(c => c.IdClient == idClient).FirstOrDefaultAsync(token);
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync(token);
    }

    public async Task<bool> ClientRegisteredByPeselAsync(string pesel, int idTrip, CancellationToken token)
    {
        var result = await _context.ClientTrips
            .Where(c => c.IdClientNavigation.Pesel == pesel && c.IdTrip == idTrip)
            .CountAsync(token);
        return result > 0;
    }

    public async Task RegisterClientForTripAsync(ClientRegistrationDto dto, int idTrip, CancellationToken token)
    {
        await _context.Clients.AddAsync(new Client()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Pesel = dto.Pesel,
            Email = dto.Email,
            Telephone = dto.Telephone
        }, token);
        await _context.SaveChangesAsync(token);

        var client = await _context.Clients.Where(c => c.Pesel == dto.Pesel).FirstOrDefaultAsync(token);

        await _context.ClientTrips.AddAsync(new ClientTrip()
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = dto.PaymentDate
        }, token);
        await _context.SaveChangesAsync(token);
    }
}