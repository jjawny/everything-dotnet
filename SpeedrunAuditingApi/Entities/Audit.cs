namespace SpeedrunAuditingApi.Models;

public class Audit
{
  public Guid? CreatedBy { get; set; }
  public Guid? UpdatedBy { get; set; }
  public DateTime? CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public bool IsDeleted { get; set; } = false;
}
