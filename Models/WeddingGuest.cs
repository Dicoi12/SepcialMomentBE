using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SepcialMomentBE.Models
{
    public class WeddingGuest
    {
        public int Id { get; set; }
        
        [Required]
        public int EventId { get; set; }
        
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? TableId { get; set; }
        public bool IsConfirmed { get; set; }
        public string? DietaryRestrictions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        [ForeignKey("EventId")]
        public Event Event { get; set; } = null!;
        public Table? Table { get; set; }
    }
} 