using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Core.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string MiddleName { get; set; } = "";

        [MaxLength(100)]
        public string? LastName { get; set; } = "";

        [Required]
        public int Age { get; set; }

        [Required]
        public string PhoneNumber { get; set; } = "";

        [Required]
        public string ClassOrCourse { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
