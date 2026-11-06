
namespace AcademyGraduationSupplyManagement.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public List<Offer> Offers { get; set; } = new List<Offer>();
    }
}
