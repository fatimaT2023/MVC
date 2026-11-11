using System.ComponentModel.DataAnnotations;

namespace task8.Models
{
    public class UserRoleDTO
    {
        public string Id { get; set; } = "";

        public string FullName { get; set; } = "";

        public string Email { get; set; } = "";

        [Required]
        public string Role { get; set; } = "User";
    }
}