namespace SpeedrunAuditingApi.Models;

public class CreditCard
{
  public Guid Id { get; set; }
  public string? HolderName { get; set; }
  public string? BankName { get; set; }
  public string? Number { get; set; }
  public string? Cvc { get; set; }
  public DateTime? Expiry { get; set; }
  public Audit Audit { get; set; } = new();
}
