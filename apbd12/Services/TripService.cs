using apbd12.Data;
using apbd12.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace apbd12.Services;

public class TripService : ITripService
{
    private readonly DatabaseContext _context;

    public TripService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<TripResponseDto> GetTripsAsync(int pageNum, int pageSize, CancellationToken token)
    {
        if (pageNum < 1) pageNum = 1;
        if (pageSize < 1) pageSize = 10;

        var totalItems = await _context.Trips.CountAsync(token);
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var trips = await _context.Trips
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDto()
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries
                    .Select(ct => new CountryDto
                    {
                        Name = ct.Name
                    }).ToList(),

                Clients = t.ClientTrips
                    .Select(ct => new ClientDto
                    {
                        FirstName = ct.IdClientNavigation.FirstName,
                        LastName = ct.IdClientNavigation.LastName,
                    }).ToList()
            }).ToListAsync(token);

        return new TripResponseDto()
        {
            Trips = trips,
            AllPages = totalPages,
            PageNum = pageNum,
            PageSize = pageSize
        };
    }

    public async Task<bool> TripExistsAsync(int idTrip, CancellationToken token)
    {
        var result = await _context.Trips.AnyAsync(t => t.IdTrip == idTrip, token);
        return result;
    }

    public async Task<bool> TripIsFutureAsync(int idTrip, CancellationToken token)
    {
        var result = await _context.Trips.Where(t => t.IdTrip == idTrip).FirstOrDefaultAsync(token);
        return result.DateFrom > DateTime.Now;
    }
}