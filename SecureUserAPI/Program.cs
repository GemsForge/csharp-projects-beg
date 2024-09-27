using Microsoft.OpenApi.Models;
using SecureUserAPI.Mappers;
using SecureUserConsole.data;
using SecureUserConsole.manager;
using SecureUserConsole.service;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    //Modify the information displayed in the UI
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Secure User Manager API",
        Description = @"Welcome to the **Secure User Management API**! This API provides comprehensive user management functionality, allowing you to register users, manage their profiles, authenticate logins, and securely handle password resets.

### Key Features:
- **User Registration**: Create new user accounts with automatic username generation.
- **Login**: Authenticate users with secure password verification.
- **Password Reset**: Safely handle password resets after failed login attempts.
- **User Management**: Retrieve, update, and remove user profiles.

### How to Get Started:
1. Use the `/api/user` endpoints to manage users.
2. Authenticate users via the `/api/auth/login` endpoint.
3. Handle password resets through the `/api/auth/password-reset` endpoint.

Happy coding! 😊",
    Contact = new OpenApiContact
        {
            Name = "SecureUserAPI Github Project",
            Url = new Uri("https://github.com/Dbrown127/csharp-projects-beg")
        }
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

string? usersFilePath = builder.Configuration.GetValue<string>("UsersFilePath");
builder.Services.AddScoped<IUserRepository>(provider => new UserRepository(usersFilePath));

// Register Password Utility (independent service)
builder.Services.AddScoped<IPasswordUtility, PasswordUtility>();

// Register services that depend on others (like IUserService, IPasswordResetService, IUserManager)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordResetService>(provider =>
    new PasswordResetService(
        provider.GetRequiredService<IPasswordUtility>(),
        provider.GetRequiredService<IUserService>()));

builder.Services.AddScoped<IUserManager>(provider =>
    new UserManager(
        provider.GetRequiredService<IUserService>(),
        provider.GetRequiredService<IPasswordUtility>()));
builder.Services.AddScoped<IUserMapper, UserMapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
