using Microsoft.EntityFrameworkCore.Diagnostics;
using SpeedrunAuditingApi.Utils;

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
    // var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value ?? Guid.Empty.ToString();
    var userId = Guid.NewGuid(); // DUMMY FOR NOW

    eventData.Context?.PerformAudit(userId);
    return base.SavingChanges(eventData, result);
  }

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancelToken = default
  )
  {
    // var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value ?? Guid.Empty.ToString();
    var userId = Guid.NewGuid(); // DUMMY FOR NOW

    eventData.Context?.PerformAudit(userId);
    return base.SavingChangesAsync(eventData, result, cancelToken);
  }
}
