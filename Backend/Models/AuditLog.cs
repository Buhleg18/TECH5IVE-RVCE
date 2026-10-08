namespace Backend.Models;

public class AuditLog
{
    public long AuditID { get; set; }

    public int? UserID { get; set; }

    public string EventType { get; set; } = string.Empty;

    public string? EventDescription { get; set; }

    public DateTime EventTimestamp { get; set; }

    public User? User { get; set; }
}