using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SepcialMomentBE.Models
{
    public class WeddingServiceProvider
    {
        public int Id { get; set; }
        
        [Required]
        public int EventId { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string ServiceType { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        public string? ContactInfo { get; set; }
        public decimal? Cost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        [ForeignKey("EventId")]
        public Event Event { get; set; } = null!;
    }
} 