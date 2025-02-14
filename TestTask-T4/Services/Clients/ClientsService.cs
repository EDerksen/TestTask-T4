using Microsoft.EntityFrameworkCore;
using TestTask_T4.Data;
using TestTask_T4.Exceptions;
using TestTask_T4.Model;

namespace TestTask_T4.Services.Clients
{
    public class ClientsService : IClientsService
    {
        private readonly ILogger<ClientsService> _logger;
        private readonly FinanceDbContext _dbContext;
        private readonly TimeProvider _timeProvider;

        public ClientsService(ILogger<ClientsService> logger, FinanceDbContext dbContext, TimeProvider timeProvider)
        {
            _logger = logger;
            _dbContext = dbContext;
            _timeProvider = timeProvider;
        }

        public async Task<ClientBalance> GetClientBalance(Guid clientId, CancellationToken cancellationToken = default)
        {
            var client = await _dbContext.Clients
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == clientId, cancellationToken);
            if(client == null)
            {
                throw new NotFoundException("Client not found", $"Can't found Client with id={clientId}");
            }

            return new ClientBalance()
            {
                Balance = client.Balance,
                BalanceDateTime = _timeProvider.GetUtcNow().UtcDateTime
            };
        }

        public async Task<Client> GetClientForUpdate(Guid clientId, CancellationToken cancellationToken = default)
        {
            var existingClient = await SelectClientForUpdate(clientId);
            if(existingClient == null)
            {
                var client = new Client()
                {
                    Id = clientId
                };

                _dbContext.Clients.Add(client);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return client;
            }

            return existingClient;
        }

        private async Task<Client?> SelectClientForUpdate(Guid clientId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Clients
                .FromSqlRaw($"SELECT * FROM \"{nameof(FinanceDbContext.Clients)}\" WHERE \"{nameof(Client.Id)}\" = @p0 FOR UPDATE", clientId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
