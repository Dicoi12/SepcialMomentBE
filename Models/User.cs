using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SepcialMomentBE.Models
{
    public class User
    {
        public Guid Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [JsonIgnore]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        
        [JsonIgnore]
        public DateTime? RefreshTokenExpiryTime { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [JsonIgnore]
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
} 