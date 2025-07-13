using System.ComponentModel.DataAnnotations;

namespace SepcialMomentBE.Models
{
    public class EventType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}