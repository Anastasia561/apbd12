using apbd12.Models;
using apbd12.Models.Dtos;

namespace apbd12.Services;

public interface IClientService
{
    Task<bool> HasTripsAsync(CancellationToken token, int idClient);
    Task<bool> ClientExistsByIdAsync(CancellationToken token, int idClient);
    Task<bool> ClientExistsByPeselAsync(string pesel, CancellationToken token);
    Task DeleteClientAsync(int idClient, CancellationToken token);
    Task<bool> ClientRegisteredByPeselAsync(string pesel, int idTrip, CancellationToken token);
    Task RegisterClientForTripAsync(ClientRegistrationDto dto, int idTrip, CancellationToken token);
}