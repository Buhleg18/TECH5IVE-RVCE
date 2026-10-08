using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthController(ApplicationDbContext db)
    {
        _db = db;
        _passwordHasher = new PasswordHasher<User>();
    }

    // REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.StudentNumber) ||
            string.IsNullOrWhiteSpace(request.FullName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new
            {
                error = "All fields are required."
            });
        }

        bool studentExists = await _db.Users
            .AnyAsync(u => u.StudentNumber == request.StudentNumber);

        if (studentExists)
        {
            return BadRequest(new
            {
                error = "Student number already exists."
            });
        }

        bool emailExists = await _db.Users
            .AnyAsync(u => u.Email == request.Email);

        if (emailExists)
        {
            return BadRequest(new
            {
                error = "Email already exists."
            });
        }

        var user = new User
        {
            StudentNumber = request.StudentNumber,
            FullName = request.FullName,
            Email = request.Email,
            RoleID = 1,
            AccountStatus = "Active",
            DateRegistered = DateTime.Now
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            request.Password
        );

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Registration successful."
        });
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(
                u => u.StudentNumber == request.StudentNumber
            );

        if (user == null)
        {
            return Unauthorized(new
            {
                error = "Invalid student number or password."
            });
        }

        if (user.AccountStatus != "Active")
        {
            return Unauthorized(new
            {
                error = "This account is suspended."
            });
        }

        var passwordResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new
            {
                error = "Invalid student number or password."
            });
        }

        user.LastLoginDate = DateTime.Now;
        await _db.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            message = "Login successful.",
            userId = user.UserID,
            studentNumber = user.StudentNumber,
            fullName = user.FullName,
            role = user.Role?.RoleName ?? "Student"
        });
    }
}

public class RegisterRequest
{
    public string StudentNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string StudentNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}