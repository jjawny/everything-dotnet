using SpeedrunAuditingApi.Contexts;

// 1. Add services into DI container
var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
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

// 3. Endpoints
app.MapGet("/api/placeholder", () =>
{
    return Results.NotFound();
});



app.Run();
