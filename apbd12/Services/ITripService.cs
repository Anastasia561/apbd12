using apbd12.Models.Dtos;

namespace apbd12.Services;

public interface ITripService
{
    public Task<TripResponseDto> GetTripsAsync(int pageNum, int pageSize, CancellationToken token);
    public Task<bool> TripExistsAsync(int idTrip, CancellationToken token);
    public Task<bool> TripIsFutureAsync(int idTrip, CancellationToken token);
}