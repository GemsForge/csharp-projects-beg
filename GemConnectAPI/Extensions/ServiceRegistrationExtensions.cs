using CommonLibrary.Data;
using FizzBuzzConsole.manager;
using FizzBuzzConsole.model;
using FizzBuzzConsole.service;
using GemConnectAPI.Mappers.SecureUser;
using GemConnectAPI.Mappers.TaskTracker;
using GemConnectAPI.Services.SecureUser;
using SecureUserConsole.manager;
using SecureUserConsole.model;
using SecureUserConsole.service;
using TaskTrackerConsole.manager;
using TaskTrackerConsole.model;
using TaskTrackerConsole.service;
using Task = TaskTrackerConsole.model.Task;

namespace GemConnectAPI.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddTaskTrackerServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Task Tracker repository and manager
            string? taskFilePath = configuration.GetValue<string>("TaskFilePath");
            services.AddScoped<IGenericRepository<Task>>(provider => new GenericJsonRepository<Task>(taskFilePath));
            services.AddScoped<ITaskManager>(provider => new TaskManager(provider.GetRequiredService<ITaskService>()));
            services.AddScoped<ITaskService>(provider => new TaskService(provider.GetRequiredService<IGenericRepository<Task>>()));
            services.AddScoped<ITaskMapper, TaskMapper>();

            return services;
        }
        //Register FizzBuzzService
        public static IServiceCollection AddFizzBuzzServices(this IServiceCollection services, IConfiguration configuration)
        {
            string? fizzBuzzFilePath = configuration.GetValue<string>("FizzBuzzFilePath");
            services.AddScoped<IFizzBuzzService>(provider => new FizzBuzzService(provider.GetRequiredService<IGenericRepository<FizzBuzzGamePlay>>()));
            services.AddScoped<IFizzBuzzManager, FizzBuzzManager>();
            services.AddScoped<IGenericRepository<FizzBuzzGamePlay>>(provider => new GenericJsonRepository<FizzBuzzGamePlay>(fizzBuzzFilePath));
            return services;
        }
        public static IServiceCollection AddUserManagementServices(this IServiceCollection services, IConfiguration configuration)
        {
            string? usersFilePath = configuration.GetValue<string>("UserFilePath");
            services.AddScoped<IGenericRepository<User>>(provider => new GenericJsonRepository<User>(usersFilePath));
            services.AddScoped<IUserService>(provider => new UserService(provider.GetRequiredService<IGenericRepository<User>>()));
            // Register Password Utility (independent service)
            services.AddScoped<IPasswordUtility, PasswordUtility>();
            // Register services that depend on others (like IUserService, IPasswordResetService, IUserManager)
            services.AddScoped<IUserManager>(provider =>
                new UserManager(
                    provider.GetRequiredService<IUserService>(),
                    provider.GetRequiredService<IPasswordUtility>())
            );
            services.AddScoped<IPasswordResetService>(provider =>
                 new PasswordResetService(
                     provider.GetRequiredService<IPasswordUtility>(),
                     provider.GetRequiredService<IUserService>()));

            // Add other services like IUserMapper, PasswordResetService, etc.
            services.AddScoped<IUserMapper, UserMapper>();
            services.AddScoped<IApiUserManager>(provider =>
                new ApiUserManager(
                    provider.GetRequiredService<IUserManager>(),
                    provider.GetRequiredService<IUserService>(),
                    provider.GetRequiredService<IConfiguration>()));

            return services;
        }
    }

}
