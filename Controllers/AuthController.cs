using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SepcialMomentBE.Models.Auth;
using SepcialMomentBE.Services;

namespace SepcialMomentBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                _logger.LogInformation("Attempting to register user with email: {Email}", request.Email);
                
                var result = await _authService.RegisterAsync(
                    request.Email,
                    request.Password,
                    request.FirstName,
                    request.LastName);

                return Ok(new { message = "Registration successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Attempting login for user: {Email}", request.Email);
                
                var response = await _authService.LoginAsync(request.Email, request.Password);
                
                _logger.LogInformation("Login successful for user: {Email}", request.Email);
                
                return Ok(new
                {
                    accessToken = response.AccessToken,
                    refreshToken = response.RefreshToken,
                    expiresAt = response.ExpiresAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user: {Email}", request.Email);
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(request.RefreshToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var result = await _authService.RevokeTokenAsync(request.RefreshToken);
                return Ok(new { message = "Token revoked successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 