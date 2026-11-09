namespace task7.Models
{
    public class Category7
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product7> Products { get; set; }
    }
}
