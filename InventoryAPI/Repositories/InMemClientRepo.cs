using System;
using System.Collections.Generic;
using System.Linq;
using InventoryAPI.Models;
using Microsoft.AspNetCore.Http.Features;

namespace InventoryAPI.Repositories
{
    public class InMemClientRepo : IClientRepository
    {
        private readonly List<Client> clients = new()
        {
            new Client
            {
                Id = Guid.NewGuid(), Name = "Doug LaRose", HorseCount = 2, Location = "Midland",
                CreatedDate = DateTimeOffset.UtcNow
            },
            new Client
            {
                Id = Guid.NewGuid(), Name = "Kelly Blouw", HorseCount = 1, Location = "Hudsonville",
                CreatedDate = DateTimeOffset.UtcNow
            },
            new Client
            {
                Id = Guid.NewGuid(), Name = "Joe LaCross", HorseCount = 3, Location = "Alma",
                CreatedDate = DateTimeOffset.UtcNow
            }
        };

        public IEnumerable<Client> GetClients() => clients;
        public Client GetClient( Guid clientID ) => clients.SingleOrDefault(client => client.Id == clientID );
        public void CreateClient(Client client) => clients.Add( client );

        public void UpdateClient(Client client)
        {
            var index = clients.FindIndex(existingClient => existingClient.Id == client.Id);
            clients[index] = client;
        }

        public void DeleteClient(Guid id)
        {
            var index = clients.FindIndex(existingClient => existingClient.Id == id);
            clients.RemoveAt(index);
        }
    }
}