using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SepcialMomentBE.Models
{
    public class EventForm
    {
        public int Id { get; set; }
        
        [Required]
        public int EventId { get; set; }
        
        [Required]
        public int FormTemplateId { get; set; }
        
        [Required]
        public string FieldValue { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        [ForeignKey("EventId")]
        public Event Event { get; set; } = null!;
        
        [ForeignKey("FormTemplateId")]
        public EventFormTemplate FormTemplate { get; set; } = null!;
    }
} 