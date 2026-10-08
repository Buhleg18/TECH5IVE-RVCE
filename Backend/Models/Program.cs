namespace Backend.Models;

public class Program
{
    public int ProgramID { get; set; }

    public int UserID { get; set; }

    public string ProgramName { get; set; } = string.Empty;

    public string SourceCode { get; set; } = string.Empty;

    public DateTime DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public User? User { get; set; }
}