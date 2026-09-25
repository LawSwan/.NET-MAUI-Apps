var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Send the bare root URL to the Swagger test page
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();
