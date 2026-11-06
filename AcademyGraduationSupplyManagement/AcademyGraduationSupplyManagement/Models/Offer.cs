
namespace AcademyGraduationSupplyManagement.Models
{
    public class Offer
    {
        public int Id { get; set; }

        public string OfferName { get; set; } = "";

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        // Foreign Key for Category
        public int CategoryId { get; set; }

        // Navigation Property for Category
        public Category Category { get; set; } = null!;

        // Foreign Key for Supplier
        public int SupplierId { get; set; }

        // Navigation Property for Supplier
        public Supplier Supplier { get; set; } = null!;
    }
}
