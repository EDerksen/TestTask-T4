using Microsoft.EntityFrameworkCore;
using TestTask_T4.Data;
using TestTask_T4.Model;

namespace TestTask_T4.Services.Clients
{
    public class ClientsService : IClientsService
    {
        private readonly ILogger<ClientsService> _logger;
        private readonly FinanceDbContext _dbContext;

        public ClientsService(ILogger<ClientsService> logger, FinanceDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Task<Client?> FindClient(Guid clientId, CancellationToken cancellationToken = default)
        {
            return _dbContext.Clients
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == clientId, cancellationToken);
        }

        public async Task<Client> GetClientForUpdate(Guid clientId, CancellationToken cancellationToken = default)
        {
            var existingClient = SelectClientForUpdate(clientId);
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

        private Client? SelectClientForUpdate(Guid clientId)
        {
            return _dbContext.Clients
                .FromSqlRaw($"SELECT * FROM \"{nameof(FinanceDbContext.Clients)}\" WHERE \"{nameof(Client.Id)}\" = @p0 FOR UPDATE", clientId)
                .FirstOrDefault();
        }
    }
}
