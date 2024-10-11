using Microsoft.EntityFrameworkCore;
using SpeedrunAuditingApi.Models;

namespace SpeedrunAuditingApi.Contexts;

public class MyContext : DbContext
{
  public DbSet<CreditCard> CreditCards { get; set; }
  private readonly object _dbPath;
  private readonly AuditInterceptor _auditInterceptor;

  public MyContext(
    DbContextOptions<MyContext> opts,
    AuditInterceptor auditInterceptor
  ) : base(opts)
  {
    var rootPath = AppDomain.CurrentDomain.BaseDirectory;
    _dbPath = Path.Combine(rootPath, "MyContext.db");
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
    modelBuilder.Entity<CreditCard>().OwnsOne(cc => cc.Audit);
  }
}
