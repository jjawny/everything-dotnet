using Microsoft.EntityFrameworkCore;
using SpeedrunAuditingApi.Models;

namespace SpeedrunAuditingApi.Contexts;

public class MyContext : DbContext
{
  public DbSet<EliteEmployee> EliteEmployees { get; set; }
  private readonly object _dbPath;
  private readonly AuditInterceptor _auditInterceptor;

  public MyContext(
    DbContextOptions<MyContext> opts,
    AuditInterceptor auditInterceptor
  ) : base(opts)
  {
    _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MyContext.db");
    _auditInterceptor = auditInterceptor;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder opts)
  {
    if (opts.IsConfigured) return;
    var connString = $"Data Source={_dbPath}";
    opts
      .UseSqlite(connString)
      .AddInterceptors(_auditInterceptor);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<EliteEmployee>(entity =>
    {
      entity.OwnsOne(e => e.Audit);
      entity.HasQueryFilter(e => !e.Audit.IsDeleted);
    });
  }
}
