using apbd12.Models;

namespace apbd12.Services;

public interface IClientService
{
    Task<bool> HasTrips(CancellationToken token, int idClient);
    Task<bool> ClientExists(CancellationToken token, int idClient);
    Task DeleteClient(CancellationToken token, int idClient);
}