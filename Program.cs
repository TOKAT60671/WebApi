using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Interfaces;

//Get the connection string from the user secrets
var builder = WebApplication.CreateBuilder(args);
IConfigurationRoot config = new ConfigurationBuilder()
     .AddUserSecrets<Program>()
     .Build();
string sqlConnectionString = config["sqlConnectionString"];
if (string.IsNullOrWhiteSpace(sqlConnectionString))
    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");

// Adding the HTTP Context accessor to be injected. This is needed by the AspNetIdentityUserRepository
// to resolve the current user.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

//Add the database communication as service
builder.Services.AddTransient<IDatabaseRepository, DatabaseCommunicationService>(o => new DatabaseCommunicationService(sqlConnectionString));

//Add BearerToken options
builder.Services.AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme).Configure(options =>
{
    options.BearerTokenExpiration = TimeSpan.FromMinutes(60);
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();