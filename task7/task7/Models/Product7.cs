namespace task7.Models
{
    public class Product7
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }


        public Category7 Category { get; set; }
        public Supplier7 Supplier { get; set; }
    }
}
