using AssessmentApi.DataAccess;

// Web service for the Week 4 Performance Assessment app.
// Endpoints: POST api/auth/login (Basic auth), GET api/items, POST api/items.
var builder = WebApplication.CreateBuilder(args);

// One shared SQLite data access object for the whole service.
builder.Services.AddSingleton<ItemData>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Swagger UI at /swagger for testing the endpoints in a browser.
    app.UseSwagger();
    app.UseSwaggerUI();

    // Send the bare root URL to the Swagger test page.
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.MapControllers();

app.Run();
