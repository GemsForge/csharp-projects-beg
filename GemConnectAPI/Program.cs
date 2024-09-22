using CommonLibrary.TaskTracker.data;
using FizzBuzzConsole.service;
using GemConnectAPI.Mappers.SecureUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
using SecureUserConsole.data;
using SecureUserConsole.service;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    //Modify the information displayed in the UI
    //    opt.SwaggerDoc("v1", new OpenApiInfo
    //    {
    //        Version = "v1",
    //        Title = "Secure User Manager API",
    //        Description = @"Welcome to the **Secure User Management API**! This API provides comprehensive user management functionality, allowing you to register users, manage their profiles, authenticate logins, and securely handle password resets.

    //### Key Features:
    //- **User Registration**: Create new user accounts with automatic username generation.
    //- **Login**: Authenticate users with secure password verification.
    //- **Password Reset**: Safely handle password resets after failed login attempts.
    //- **User Management**: Retrieve, update, and remove user profiles.

    //### How to Get Started:
    //1. Use the `/api/user` endpoints to manage users.
    //2. Authenticate users via the `/api/auth/login` endpoint.
    //3. Handle password resets through the `/api/auth/password-reset` endpoint.

    //Happy coding! 😊",
    //        Contact = new OpenApiContact
    //        {
    //            Name = "SecureUserAPI Github Project",
    //            Url = new Uri("https://github.com/Dbrown127/csharp-projects-beg")
    //        }
    //    });
    // opt.SwaggerDoc("v1", new OpenApiInfo
    //{
    //    Version = "v1",
    //    Title = "Task Tracker API",
    //    Description = "An .NET Core Web API for managing Task items.",
    //    Contact = new OpenApiContact
    //    {
    //        Name = "TaskTrackAPI Github Project",
    //        Url = new Uri("https://github.com/Dbrown127/csharp-projects-beg")
    //    }
    //});
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
//Register FizzBuzzService
builder.Services.AddSingleton<IFizzBuzzService, FizzBuzzService>();
string? usersFilePath = builder.Configuration.GetValue<string>("UsersFilePath");
builder.Services.AddScoped<IUserRepository>(provider => new UserRepository(usersFilePath));
string? taskFilePath = builder.Configuration.GetValue<string>("TaskFilePath");
//Register Task Tracker repository and manager
builder.Services.AddScoped<ITaskRepository>(provider => new TaskRepository(taskFilePath));
builder.Services.AddScoped<ITaskManager, TaskManager>();


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
builder.Services.AddAuthorization(options => {
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
