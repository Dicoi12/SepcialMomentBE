namespace SepcialMomentBE.Models.Auth
{
    public class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty; 
        public Guid Id { get; set; } = Guid.Empty;
    }
} 