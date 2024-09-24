using Microsoft.OpenApi.Models;
using System.Reflection;
using TaskTrackerConsole.data;

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
        Title = "Task Tracker API",
        Description = "An .NET Core Web API for managing Task items.",
        Contact = new OpenApiContact
        {
            Name = "Github - TaskTrackAPI Project",
            Url = new Uri("https://github.com/Dbrown127/csharp-projects-beg")
        }
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddRazorPages();
string? taskFilePath = builder.Configuration.GetValue<string>("TaskFilePath");
//Register Task Tracker repository and manager
builder.Services.AddScoped<ITaskRepository>(provider => new TaskRepository(taskFilePath));
builder.Services.AddScoped<ITaskManager, TaskManager>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
// Enable Razor Pages
app.MapRazorPages();
app.MapControllers();


app.Run();
