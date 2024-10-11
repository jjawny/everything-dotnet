using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpeedrunAuditingApi;
using SpeedrunAuditingApi.Contexts;
using SpeedrunAuditingApi.Models;

// DI CONTAINER
var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<AuditInterceptor>();
    builder.Services.AddDbContext<MyContext>();
}

// HTTP pipeline a.k.a
// █▀▄▀█ █ █▀▄ █▀▄ █░░ █▀▀ █░█░█ ▄▀█ █▀█ █▀▀
// █░▀░█ █ █▄▀ █▄▀ █▄▄ ██▄ ▀▄▀▄▀ █▀█ █▀▄ ██▄
var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
}

// ░█████╗░██████╗░██╗░░░██╗██████╗░
// ██╔══██╗██╔══██╗██║░░░██║██╔══██╗
// ██║░░╚═╝██████╔╝██║░░░██║██║░░██║
// ██║░░██╗██╔══██╗██║░░░██║██║░░██║
// ╚█████╔╝██║░░██║╚██████╔╝██████╔╝
// ░╚════╝░╚═╝░░╚═╝░╚═════╝░╚═════╝░
// skip [validating, sanitizing, mapping, pagination, error handling, ...] as not the focus here
app.MapGet("/api/eliteemployees/{id:guid}", async (
    Guid id,
    [FromServices] MyContext ctx
) =>
{
    var card = await ctx.EliteEmployees.AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();
    return card == null ? Results.NotFound() : Results.Ok(card);
});

app.MapGet("/api/eliteemployees", async (
    [FromServices] MyContext ctx,
    [FromQuery] bool isIncludeSoftDeleted = false
) =>
{
    var query = ctx.EliteEmployees.AsNoTracking().AsQueryable();
    if (isIncludeSoftDeleted) query = query.IgnoreQueryFilters();
    var allCards = await query.ToListAsync();
    return allCards.Count > 0 ? Results.Ok(allCards) : Results.NoContent();
});

app.MapPost("/api/eliteemployees", async (
    [FromBody] EliteEmployee newEmployee,
    [FromServices] MyContext ctx
) =>
{
    var isNameTaken = await ctx.EliteEmployees.AnyAsync(e => string.Equals(e.Name, newEmployee.Name)); // efcore translates this to case-insensitive by default in SQLite
    if (isNameTaken) return Results.Conflict($"Name '{newEmployee.Name}' taken, we get confused when there's 1+ employees with the same name 🤡");
    ctx.EliteEmployees.Add(newEmployee);
    await ctx.SaveChangesAsync();
    return Results.Created($"/api/eliteemployees/{newEmployee.Id}", newEmployee);
});

app.MapPatch("/api/eliteemployees/{id:guid}", async (
    Guid id,
    [FromBody] EliteEmployee newEmployeeDetails,
    [FromServices] MyContext ctx
) =>
{
    var currEmployee = await ctx.EliteEmployees.FindAsync(id);
    if (currEmployee == null) return Results.NotFound();
    currEmployee.Name = newEmployeeDetails.Name;
    currEmployee.IsElite = newEmployeeDetails.IsElite;
    await ctx.SaveChangesAsync();
    return Results.Ok(currEmployee);
});

app.MapDelete("/api/eliteemployees/{id:guid}", async (
    Guid id,
    [FromServices] MyContext ctx
) =>
{
    var currEmployee = await ctx.EliteEmployees.FindAsync(id);
    if (currEmployee == null) return Results.NotFound();
    ctx.EliteEmployees.Remove(currEmployee);
    await ctx.SaveChangesAsync();
    return Results.Ok();
});

app.MapDelete("/api/nuke", async (
    [FromServices] MyContext ctx
) =>
{
    var allEmployees = await ctx.EliteEmployees.IgnoreQueryFilters().ToListAsync();
    var count = allEmployees.Count();
    ctx.EliteEmployees.RemoveRange(allEmployees);
    await ctx.SaveChangesAsync();
    return Results.Ok($"R.I.P those {count} guys");
});

app.Run();
