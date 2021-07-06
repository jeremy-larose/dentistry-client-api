using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryAPI.Models;

namespace InventoryAPI.Repositories
{
    public interface IClientRepository
    {
        Task<Client> GetClientAsync( Guid clientID );
        Task<IEnumerable<Client>> GetClientsAsync();

        Task CreateClientAsync(Client client);
        Task UpdateClientAsync(Client client);
        Task DeleteClientAsync(Guid id);
    }
}