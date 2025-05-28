using apbd12.Models.Dtos;

namespace apbd12.Services;

public interface ITripService
{
    public Task<TripResponseDto> GetTripsAsync(int pageNum, int pageSize, CancellationToken token);
}