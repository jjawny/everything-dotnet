using SpeedrunAuditingApi;
using SpeedrunAuditingApi.Contexts;

// 1. Add services into DI container
var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped<AuditInterceptor>();
    builder.Services.AddDbContext<MyContext>();
}

// 2. Configure the HTTP request (middleware) pipeline.
var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
}

// 3. CRUD
app.MapGet("/api/creditcards", () =>
{
    return Results.NotFound();
});

app.MapPost("/api/creditcards", () =>
{
    return Results.NotFound();
});

app.MapPatch("/api/creditcards/{id}", () =>
{
    return Results.NotFound();
});

app.MapDelete("/api/creditcards/{id}", () =>
{
    return Results.NotFound();
});

app.MapGet("/api/nuke", () =>
{
    return Results.NotFound();
});


app.Run();
