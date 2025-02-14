using TestTask_T4.Model;

namespace TestTask_T4.Services.Clients
{
    public interface IClientsService
    {
        Task<Client> GetClientForUpdate(Guid clientId, CancellationToken cancellationToken = default);
        Task<ClientBalance> GetClientBalance(Guid clientId, CancellationToken cancellationToken = default);
    }
}
