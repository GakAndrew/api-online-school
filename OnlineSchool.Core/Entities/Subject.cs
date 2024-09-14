using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Core.Entities
{
    public class Subject
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        public bool IsActive { get; set; } = true;

        public int TeacherId {  get; set; } 

        public Teacher? Teacher { get; set; }
    }
}