using System.ComponentModel.DataAnnotations;

namespace SpeedrunAuditingApi.Models;

public class EliteEmployee
{
  public Guid Id { get; set; }

  [Required]
  public string Name { get; set; } = !null;
  public bool IsElite { get; set; } = true;
  public Audit Audit { get; set; } = new();
}
