using System;

namespace InventoryAPI.Models
{
    public class Client
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public int HorseCount { get; set; }
        public string Location { get; set; }
        public DateTimeOffset CreatedDate { get; init; }
        public DateTimeOffset LastVisitDate { get; set; }
    }
}