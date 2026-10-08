namespace Backend.Models;

public class User
{
    public int UserID { get; set; }

    public string StudentNumber { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public int RoleID { get; set; }

    public DateTime DateRegistered { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public string AccountStatus { get; set; } = "Active";

    public Role? Role { get; set; }
}