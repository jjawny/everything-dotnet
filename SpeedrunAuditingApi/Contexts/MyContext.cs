using System;
using Microsoft.EntityFrameworkCore;

namespace SpeedrunAuditingApi.Contexts;

public class MyContext : DbContext
{
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
}
