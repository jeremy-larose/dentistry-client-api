using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryAPI.DTOs;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    [ApiController]
    [Route("clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _repository;

        public ClientsController(IClientRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<ClientDTO>> GetClientsAsync()
        {
            var clients = ( await _repository.GetClientsAsync())
                .Select(client => client.AsDTO());

            return clients;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetClientAsync(Guid id)
        {
            var client = await _repository.GetClientAsync(id);

            if (client is null)
            {
                return NotFound();
            }

            return client.AsDTO();
        }

        [HttpPost]
        public async Task<ActionResult<ClientDTO>> CreateClientAsync(CreateClientDTO clientDTO)
        {
            Client client = new()
            {
                Id = Guid.NewGuid(),
                Name = clientDTO.Name,
                HorseCount = clientDTO.HorseCount,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _repository.CreateClientAsync(client);

            return CreatedAtAction(nameof(GetClientAsync), new {id = client.Id}, client.AsDTO());
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateClientAsync(Guid id, UpdateClientDTO clientDTO)
        {
            var existingClient = await _repository.GetClientAsync(id);

            if (existingClient is null)
            {
                return NotFound();
            }

            Client updatedClient = existingClient with
            {
                Name = clientDTO.Name,
                HorseCount = clientDTO.HorseCount
            };

            await _repository.UpdateClientAsync(updatedClient);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClient(Guid id)
        {
            var existingClient = await _repository.GetClientAsync(id);
            if (existingClient is null)
            {
                return NotFound();
            }

            await _repository.DeleteClientAsync(id);
            return NoContent();
        }
    }
}