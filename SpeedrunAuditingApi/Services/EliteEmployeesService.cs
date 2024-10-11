using Microsoft.EntityFrameworkCore;
using SpeedrunAuditingApi.Contexts;
using SpeedrunAuditingApi.Models;

namespace SpeedrunAuditingApi.Services;

// Yes yes this should be a repo layer but not the focus for this speedrun
// (some would argue EF itself is the repo layer)
// also skip [validating, sanitizing, mapping, pagination, error handling, ...] as not the focus for this speedrun
public class EliteEmployeesService
{
  private readonly MyContext _ctx;

  public EliteEmployeesService(MyContext ctx)
  {
    _ctx = ctx;
  }

  public async Task<bool> IsExistsAsync(Guid id)
  {
    return await _ctx.EliteEmployees.AnyAsync(e => e.Id == id);
  }

  public async Task<EliteEmployee?> FindAsync(Guid id)
  {
    var eliteEmployee = await _ctx.EliteEmployees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    return eliteEmployee;
  }

  public async Task<List<EliteEmployee>> GetAllAsync(bool isIncludeSoftDeleted = false)
  {
    var query = _ctx.EliteEmployees.AsNoTracking().AsQueryable();
    if (isIncludeSoftDeleted) query = query.IgnoreQueryFilters();
    var allEmployees = await query.ToListAsync();
    return allEmployees;
  }

  public async Task<EliteEmployee> CreateAsync(EliteEmployee newEmployee)
  {
    var isNameTaken = await _ctx.EliteEmployees.AnyAsync(e => string.Equals(e.Name, newEmployee.Name)); // efcore translates this to case-insensitive by default in SQLite
    if (isNameTaken) throw new ArgumentException($"Name '{newEmployee.Name}' is taken, we get confused when there's 1+ employees with the same name 🤡");
    _ctx.EliteEmployees.Add(newEmployee);
    await _ctx.SaveChangesAsync();
    return newEmployee; // obj ref is populated w id (no need to refetch)
  }

  public async Task<EliteEmployee> UpdateAsync(Guid id, bool isElite)
  {
    var employee = await _ctx.EliteEmployees.FindAsync(id);
    if (employee == null) throw new ArgumentException($"Employee with id '{id}' not found");
    employee.IsElite = isElite;
    _ctx.Update(employee);
    await _ctx.SaveChangesAsync();
    return employee;
  }

  public async Task DeleteAsync(Guid id)
  {
    var employee = await _ctx.EliteEmployees.FindAsync(id);
    if (employee == null) throw new ArgumentException($"Employee with id '{id}' not found");
    _ctx.EliteEmployees.Remove(employee);
    await _ctx.SaveChangesAsync();
    return;
  }

  public async Task<int> DeleteAllAsync()
  {
    var allEmployees = await _ctx.EliteEmployees.IgnoreQueryFilters().ToListAsync();
    var count = allEmployees.Count();
    _ctx.EliteEmployees.RemoveRange(allEmployees);
    await _ctx.SaveChangesAsync();
    return count;
  }
}
