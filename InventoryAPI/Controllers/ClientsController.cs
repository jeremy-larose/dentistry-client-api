using System;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<ClientDTO> GetClients()
        {
            var clients = _repository.GetClients().Select(client => client.AsDTO());

            return clients;
        }

        [HttpGet("{id}")]
        public ActionResult<ClientDTO> GetClient(Guid id)
        {
            var client = _repository.GetClient(id);

            if (client is null)
            {
                return NotFound();
            }

            return client.AsDTO();
        }

        [HttpPost]
        public ActionResult<ClientDTO> CreateClient(CreateClientDTO clientDTO)
        {
            Client client = new()
            {
                Id = Guid.NewGuid(),
                Name = clientDTO.Name,
                HorseCount = clientDTO.HorseCount,
                CreatedDate = DateTimeOffset.UtcNow
            };

            _repository.CreateClient(client);

            return CreatedAtAction(nameof(GetClient), new {id = client.Id}, client.AsDTO());
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateClient(Guid id, UpdateClientDTO clientDTO)
        {
            var existingClient = _repository.GetClient(id);

            if (existingClient is null)
            {
                return NotFound();
            }

            Client updatedClient = existingClient with
            {
                Name = clientDTO.Name,
                HorseCount = clientDTO.HorseCount
            };

            _repository.UpdateClient(updatedClient);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteClient(Guid id)
        {
            var existingClient = _repository.GetClient(id);
            if (existingClient is null)
            {
                return NotFound();
            }

            _repository.DeleteClient(id);
            return NoContent();
        }
    }
}