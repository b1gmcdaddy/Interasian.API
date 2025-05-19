using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Interasian.API.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Interasian.API.DTOs;
using Interasian.API.Repositories;
using Serilog;

namespace Interasian.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthRepository authRepository, ILogger<AuthController> logger)
        {
            _authRepository = authRepository;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            try
            {
                var (succeeded, message) = await _authRepository.RegisterUserAsync(model);

                if (succeeded)
                {
                    _logger.LogInformation("User registered successfully: {Email}", model.Email);
                    return Ok(new { Message = message });
                }

                _logger.LogWarning("User registration failed: {Email}, Reason: {Message}", model.Email, message);
                return BadRequest(new { Message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration for email: {Email}", model.Email);
                return StatusCode(500, new { Message = "Internal server error during registration" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var (succeeded, token) = await _authRepository.LoginUserAsync(model);

                if (succeeded && token != null)
                {
                    _logger.LogInformation("User logged in successfully: {Email}", model.Email);
                    return Ok(new { Token = token });
                }

                _logger.LogWarning("Login failed for user: {Email}", model.Email);
                return Unauthorized(new { Message = "Invalid credentials." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login attempt for email: {Email}", model.Email);
                return StatusCode(500, new { Message = "Internal server error during login" });
            }
        }
    }
}