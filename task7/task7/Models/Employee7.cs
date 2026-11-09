using Microsoft.CodeAnalysis;

namespace task7.Models
{
    public class Employee7
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
