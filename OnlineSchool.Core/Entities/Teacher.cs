
using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Core.Entities
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string Patronymic { get; set; } = "";

        public string Description { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Phone]
        public string PhoneNumber { get; set; } = "";

        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        public List<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
