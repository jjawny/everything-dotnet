using Microsoft.AspNetCore.Mvc;
using SpeedrunAuditingApi;
using SpeedrunAuditingApi.Contexts;
using SpeedrunAuditingApi.Models;
using SpeedrunAuditingApi.Services;

// DI CONTAINER
var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<AuditInterceptor>();
    builder.Services.AddDbContext<MyContext>();
    builder.Services.AddScoped<EliteEmployeesService>();
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
// --------------------------------
// Opinionated minimal API example:
// --------------------------------
// app.MapGet("/api/friends/{id}",
//   [...Filters]
//   [AllowAnonymous]
//   [Authorize(Roles = "MyAzureHomie")]
//   async (
//     Guid urlPathParams,
//     [FromQuery] string? urlQueryParams
//     [FromBody] MyDto thePayload,
//     [FromServices] MyService myService
//   ) =>
//   {
//      biz logic
//   });
app.MapGet("/api/eliteemployees/{id:guid}", async (
    Guid id,
    [FromServices] EliteEmployeesService eliteEmployeesService
) =>
{
    var eliteEmployee = await eliteEmployeesService.FindAsync(id);
    var res = eliteEmployee == null ? Results.NotFound() : Results.Ok(eliteEmployee);
    return res;
});

app.MapGet("/api/eliteemployees", async (
    [FromServices] EliteEmployeesService eliteEmployeesService,
    [FromQuery] bool isIncludeSoftDeleted = false
) =>
{
    var eliteEmployees = await eliteEmployeesService.GetAllAsync();
    var res = eliteEmployees.Count > 0 ? Results.Ok(eliteEmployees) : Results.NoContent();
    return res;
});

app.MapPost("/api/eliteemployees", async (
    [FromBody] EliteEmployee newEmployee,
    [FromServices] EliteEmployeesService eliteEmployeesService
) =>
{
    try
    {
        var employee = await eliteEmployeesService.CreateAsync(newEmployee);
        var res = Results.Created($"/api/eliteemployees/{employee.Id}", employee);
        return res;
    }
    catch (ArgumentException ex)
    {
        // prefer result pattern here
        var is404 = ex.Message.Contains("not found", StringComparison.InvariantCultureIgnoreCase);
        if (is404) return Results.NotFound(ex.Message);
        else return Results.BadRequest(ex.Message);
    }
});

app.MapPatch("/api/eliteemployees/{id:guid}", async (
    Guid id,
    [FromBody] bool isElite,
    [FromServices] EliteEmployeesService eliteEmployeesService
) =>
{
    try
    {
        var employee = await eliteEmployeesService.UpdateAsync(id, isElite);
        var res = Results.Ok(employee);
        return res;
    }
    catch (ArgumentException ex)
    {
        // prefer result pattern here
        var is404 = ex.Message.Contains("not found", StringComparison.InvariantCultureIgnoreCase);
        if (is404) return Results.NotFound(ex.Message);
        else return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/api/eliteemployees/{id:guid}", async (
    Guid id,
    [FromServices] EliteEmployeesService eliteEmployeesService
) =>
{
    try
    {
        await eliteEmployeesService.DeleteAsync(id);
        var res = Results.Ok();
        return res;
    }
    catch (ArgumentException ex)
    {
        // prefer result pattern here
        var is404 = ex.Message.Contains("not found", StringComparison.InvariantCultureIgnoreCase);
        if (is404) return Results.NotFound(ex.Message);
        else return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/api/nuke", async (
    [FromServices] EliteEmployeesService eliteEmployeesService
) =>
{
    var totalDeleted = await eliteEmployeesService.DeleteAllAsync();
    var res = Results.Ok($"R.I.P those {totalDeleted} guys");
});

app.Run();
