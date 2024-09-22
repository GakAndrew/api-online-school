using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Core.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public int UserId { get; set; }

        public User? User { get; set; }
    }
}
