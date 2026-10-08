namespace Backend.Models;

public class ExecutionSession
{
    public int SessionID { get; set; }

    public int ProgramID { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Outcome { get; set; }

    public Program? Program { get; set; }
}