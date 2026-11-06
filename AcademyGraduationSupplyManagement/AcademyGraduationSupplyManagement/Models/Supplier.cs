
namespace AcademyGraduationSupplyManagement.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public Offer? Offer { get; set; }
    }
}
