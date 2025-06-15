using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SepcialMomentBE.Models
{
    public class Table
    {
        public int Id { get; set; }
        
        [Required]
        public int EventId { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int Capacity { get; set; }
        
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        [ForeignKey("EventId")]
        public Event Event { get; set; } = null!;
        public ICollection<WeddingGuest> Guests { get; set; } = new List<WeddingGuest>();
    }
} 