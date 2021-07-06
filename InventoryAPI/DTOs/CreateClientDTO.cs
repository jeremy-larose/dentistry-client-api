using System.ComponentModel.DataAnnotations;

namespace InventoryAPI.DTOs
{
    public class CreateClientDTO
    {
        [Required]
        public string Name { get; init; }
        [Required]
        [Range(1, 100)]
        public int HorseCount { get; init; }
    }
}