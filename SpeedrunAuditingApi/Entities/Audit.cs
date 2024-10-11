namespace SpeedrunAuditingApi.Models;

public class Audit
{
  public Guid? CreatedBy { get; set; }
  public Guid? UpdatedBy { get; set; }
  public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; set; }
  public bool IsDeleted { get; set; } = false;
}
