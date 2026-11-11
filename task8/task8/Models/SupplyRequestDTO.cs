using System.ComponentModel.DataAnnotations;

namespace task8.Models
{
    public class SupplyRequestDTO
    {
        [Required]
        public List<RequestItemDTO> Items { get; set; } = new();
    }

    public class RequestItemDTO
    {
        public int ProductId { get; set; }

        [Range(0, 100000)]
        public int Quantity { get; set; }
    }
}