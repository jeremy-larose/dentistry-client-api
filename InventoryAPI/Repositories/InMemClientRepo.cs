using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryAPI.Models;

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

        public async Task<IEnumerable<Client>> GetClientsAsync() => await Task.FromResult( clients );

        public async Task<Client> GetClientAsync(Guid clientID)
        {
            var client = clients.SingleOrDefault(client => client.Id == clientID);
            return await Task.FromResult(client);
        }

        public async Task CreateClientAsync(Client client)
        {
            clients.Add(client);
            await Task.CompletedTask;
        }

        public async Task UpdateClientAsync(Client client)
        {
            var index = clients.FindIndex(existingClient => existingClient.Id == client.Id);
            clients[index] = client;
            await Task.CompletedTask;
        }

        public async Task DeleteClientAsync(Guid id)
        {
            var index = clients.FindIndex(existingClient => existingClient.Id == id);
            clients.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}