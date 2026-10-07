using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Backend.Models.Program> Programs { get; set; }
    public DbSet<ExecutionSession> ExecutionSessions { get; set; }
    public DbSet<ExecutionLog> ExecutionLogs { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table names
        modelBuilder.Entity<Role>().ToTable("Role");
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Backend.Models.Program>().ToTable("Program");
        modelBuilder.Entity<ExecutionSession>().ToTable("ExecutionSession");
        modelBuilder.Entity<ExecutionLog>().ToTable("ExecutionLog");
        modelBuilder.Entity<AuditLog>().ToTable("AuditLog");

        // Primary keys
        modelBuilder.Entity<Role>()
            .HasKey(r => r.RoleID);

        modelBuilder.Entity<User>()
            .HasKey(u => u.UserID);

        modelBuilder.Entity<Backend.Models.Program>()
            .HasKey(p => p.ProgramID);

        modelBuilder.Entity<ExecutionSession>()
            .HasKey(s => s.SessionID);

        modelBuilder.Entity<ExecutionLog>()
            .HasKey(l => l.LogID);

        modelBuilder.Entity<AuditLog>()
            .HasKey(a => a.AuditID);

        // Role -> Users
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleID);

        // User -> Programs
        modelBuilder.Entity<Backend.Models.Program>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserID);

        // Program -> Execution Sessions
        modelBuilder.Entity<ExecutionSession>()
            .HasOne(s => s.Program)
            .WithMany()
            .HasForeignKey(s => s.ProgramID);

        // Execution Session -> Execution Logs
        modelBuilder.Entity<ExecutionLog>()
            .HasOne(l => l.Session)
            .WithMany()
            .HasForeignKey(l => l.SessionID);

        // User -> Audit Logs
        modelBuilder.Entity<AuditLog>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserID)
            .IsRequired(false);
    }
}