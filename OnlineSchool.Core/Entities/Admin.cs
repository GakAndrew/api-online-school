using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Core.Entities
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = "";
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = "";

        [MaxLength(100)]
        public string Patronymic { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Phone]
        public string PhoneNumber { get; set; } ="";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
