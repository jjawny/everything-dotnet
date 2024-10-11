using Microsoft.EntityFrameworkCore;
using SpeedrunAuditingApi.Models;

namespace SpeedrunAuditingApi.Contexts;

public class MyContext : DbContext
{
  public DbSet<CreditCard> CreditCards { get; set; }
  private readonly object _dbPath;

  public MyContext(DbContextOptions<MyContext> opts) : base(opts)
  {
    var rootPath = AppDomain.CurrentDomain.BaseDirectory;
    _dbPath = Path.Combine(rootPath, "MyContext.db");
  }

  protected override void OnConfiguring(DbContextOptionsBuilder opts)
  {
    if (opts.IsConfigured) return;
    var connString = $"Data Source={_dbPath}";
    opts.UseSqlite(connString);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<CreditCard>().OwnsOne(cc => cc.Audit);
  }
}
