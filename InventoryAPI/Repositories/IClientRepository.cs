using System;
using System.Collections.Generic;
using InventoryAPI.Models;

namespace InventoryAPI.Repositories
{
    public interface IClientRepository
    {
        Client GetClient( Guid clientID );
        IEnumerable<Client> GetClients();

        void CreateClient(Client client);
        void UpdateClient(Client client);
        void DeleteClient(Guid id);
    }
}