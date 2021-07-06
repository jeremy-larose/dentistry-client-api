using InventoryAPI.DTOs;
using InventoryAPI.Models;

namespace InventoryAPI
{
    public static class Extensions
    {
        public static ClientDTO AsDTO(this Client client)
        {
            return new ClientDTO()
            {
                Id = client.Id,
                Name = client.Name,
                HorseCount = client.HorseCount,
                CreatedDate = client.CreatedDate
            };
        }
    }
}