using System;
using System.Collections.Generic;
using InventoryAPI.Models;

namespace InventoryAPI.Repositories
{
    public class InMemClientRepo
    {
        private readonly List<Client> items = new()
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

        public IEnumerable<Client> GetClients()
        {
            
        }
        
    }
}