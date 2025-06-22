using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SepcialMomentBE.Models
{
    public class Event
    {
        public int Id { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public string Location { get; set; } = string.Empty;
        
        [Required]
        public string EventType { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        
        public ICollection<EventForm> EventForms { get; set; } = new List<EventForm>();
        public ICollection<WeddingExpense> WeddingExpenses { get; set; } = new List<WeddingExpense>();
        public ICollection<WeddingGuest> WeddingGuests { get; set; } = new List<WeddingGuest>();
        public ICollection<Table> Tables { get; set; } = new List<Table>();
        public ICollection<InvitationTemplate> InvitationTemplates { get; set; } = new List<InvitationTemplate>();
        public ICollection<WeddingServiceProvider> ServiceProviders { get; set; } = new List<WeddingServiceProvider>();
    }

    public class CreateEventWithFormRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public string Location { get; set; } = string.Empty;
        
        [Required]
        public string EventType { get; set; } = string.Empty;
        
        public List<EventFormData> FormData { get; set; } = new List<EventFormData>();
    }

    public class EventFormData
    {
        [Required]
        public int FormTemplateId { get; set; }
        
        [Required]
        public string FieldValue { get; set; } = string.Empty;
    }
} 