using WebApi.Services;
using WebApi.Repositories;
using WebApi.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var sqlConnectionString = builder.Configuration["SqlConnectionString"];
//builder.Services.AddTransient<ISaveFileRepository, SaveFileRepository>(o => new SaveFileRepository(sqlConnectionString));
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString); 



builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/", () => $"The API is up . Connection string found: {(sqlConnectionStringFound ? "" : "")}");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
