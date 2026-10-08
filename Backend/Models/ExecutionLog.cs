namespace Backend.Models;

public class ExecutionLog
{
    public long LogID { get; set; }

    public int SessionID { get; set; }

    public int StepNumber { get; set; }

    public int ProgramCounterValue { get; set; }

    public string InstructionExecuted { get; set; } = string.Empty;

    public string RegisterSnapshot { get; set; } = string.Empty;

    public string? MemorySnapshot { get; set; }

    public DateTime LogTimestamp { get; set; }

    public ExecutionSession? Session { get; set; }
}