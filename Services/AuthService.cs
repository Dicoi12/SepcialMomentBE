using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SepcialMomentBE.Data;
using SepcialMomentBE.Models;
using SepcialMomentBE.Models.Auth;

namespace SepcialMomentBE.Services
{
    public interface IAuthService
    {
        Task<TokenResponse> LoginAsync(string email, string password);
        Task<TokenResponse> RefreshTokenAsync(string refreshToken);
        Task<bool> RegisterAsync(string email, string password, string firstName, string lastName);
        Task<bool> RevokeTokenAsync(string refreshToken);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 32; // 256 bits
        private const int Iterations = 10000;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenResponse> LoginAsync(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password");
            }

            return await GenerateTokensAsync(user);
        }

        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);

            if (user == null)
            {
                throw new Exception("Invalid refresh token");
            }

            return await GenerateTokensAsync(user);
        }

        public async Task<bool> RegisterAsync(string email, string password, string firstName, string lastName)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                throw new Exception("Email already exists");
            }

            CreatePasswordHash(password, out byte[] passwordHash);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null)
            {
                return false;
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<TokenResponse> GenerateTokensAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim("userId", user.Id.ToString()),
                    new Claim("email", user.Email),
                    new Claim("firstName", user.FirstName),
                    new Claim("lastName", user.LastName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new TokenResponse
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken,
                ExpiresAt = tokenDescriptor.Expires!.Value
            };
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Iterations,
                HashAlgorithmName.SHA256);
            
            var salt = pbkdf2.Salt;
            var hash = pbkdf2.GetBytes(HashSize);
            
            // Combinăm salt-ul și hash-ul într-un singur array
            passwordHash = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, passwordHash, 0, SaltSize);
            Array.Copy(hash, 0, passwordHash, SaltSize, HashSize);
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash)
        {
            // Extragem salt-ul din hash-ul stocat
            var salt = new byte[SaltSize];
            Array.Copy(storedHash, 0, salt, 0, SaltSize);

            // Calculăm hash-ul pentru parola furnizată folosind același salt
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256);
            
            var hash = pbkdf2.GetBytes(HashSize);

            // Comparăm hash-urile
            for (int i = 0; i < HashSize; i++)
            {
                if (storedHash[i + SaltSize] != hash[i])
                    return false;
            }
            return true;
        }
    }
} 