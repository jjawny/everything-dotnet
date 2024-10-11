
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SpeedrunAuditingApi.Models;

namespace SpeedrunAuditingApi.Utils;

public static class AuditUtils
{
  public static void PerformAudit(this DbContext ctx, Guid? userId)
  {
    if (ctx == null) return;

    foreach (var entry in ctx.ChangeTracker.Entries())
    {
      _ = entry.State switch
      {
        EntityState.Deleted => AuditDelete(entry, userId),
        EntityState.Modified => AuditModify(entry, userId),
        EntityState.Added => AuditAdd(entry, userId),
        _ => false
      };
    }
  }

  private static bool AuditDelete(EntityEntry entry, Guid? userId)
  {
    if (entry.Entity is Audit entity)
    {
      entry.State = EntityState.Modified;
      entity.UpdatedAt = DateTime.UtcNow;
      entity.UpdatedBy = userId;
      entity.IsDeleted = true;

      entry.Property(nameof(entity.CreatedBy)).IsModified = false;
      entry.Property(nameof(entity.CreatedAt)).IsModified = false;

      return true;
    }
    return false;
  }

  private static bool AuditModify(EntityEntry entry, Guid? userId)
  {
    if (entry.Entity is Audit entity)
    {
      entity.UpdatedAt = DateTime.UtcNow;
      entity.UpdatedBy = userId;

      entry.Property(nameof(entity.CreatedBy)).IsModified = false;
      entry.Property(nameof(entity.CreatedAt)).IsModified = false;
      entry.Property(nameof(entity.IsDeleted)).IsModified = false;

      return true;
    }
    return false;
  }

  private static bool AuditAdd(EntityEntry entry, Guid? userId)
  {
    if (entry.Entity is Audit entity)
    {
      entity.CreatedAt = DateTime.UtcNow;
      entity.CreatedBy = userId;

      entry.Property(nameof(entity.UpdatedBy)).IsModified = false;
      entry.Property(nameof(entity.UpdatedAt)).IsModified = false;
      entry.Property(nameof(entity.IsDeleted)).IsModified = false;

      return true;
    }
    return false;
  }
}
