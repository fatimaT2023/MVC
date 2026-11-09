namespace task7.Models
{
    public class Supplier7
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public ICollection<Product7> Products { get; set; }
    }
}
