using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SpeedrunAuditingApi;

public class AuditInterceptor : SaveChangesInterceptor
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public override InterceptionResult<int> SavingChanges(
    DbContextEventData eventData,
    InterceptionResult<int> result
  )
  {
    // TODO: audit
    return base.SavingChanges(eventData, result);
  }

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancelToken = default
  )
  {
    // TODO: audit
    return base.SavingChangesAsync(eventData, result, cancelToken);
  }
}
