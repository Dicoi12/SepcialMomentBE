using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SepcialMomentBE.Models
{
    public class EventFormTemplate
    {
        public int Id { get; set; }

        public int? EventTypeId { get; set; }
        
        [ForeignKey("EventTypeId")]
        public EventType? EventType { get; set; }

        [Required]
        public string FieldName { get; set; } = string.Empty;
        
        [Required]
        public string FieldType { get; set; } = string.Empty; // text, number, date, select, etc.
        
        public string? Description { get; set; }
        public bool IsRequired { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public string? FieldLabel { get; set; }
        
        public int DisplayOrder { get; set; }
        
        public string? Options { get; set; } // JSON pentru câmpuri de tip select/radio/checkbox
        
        public string? ValidationRules { get; set; } // JSON pentru reguli de validare
        
        public string? HelpText { get; set; }

        // Navigation properties
        public ICollection<EventForm> EventForms { get; set; } = new List<EventForm>();
    }
} 