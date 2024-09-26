using CommonLibrary.Data;
using FizzBuzzConsole.model;
using FizzBuzzConsole.service;
using GemConnectAPI.Mappers.SecureUser;
using GemConnectAPI.Mappers.TaskTracker;
using GemConnectAPI.Services.SecureUser;
using SecureUserConsole.model;
using SecureUserConsole.service;
using TaskTrackerConsole.data;
using TaskTrackerConsole.model;
using Task = TaskTrackerConsole.model.Task;

namespace GemConnectAPI.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddTaskTrackerServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Task Tracker repository and manager
            string? taskFilePath = configuration.GetValue<string>("SharedJosn");
            services.AddScoped<ISharedRepository<TaskWrapper, Task>>(provider => new JsonSharedRepository<TaskWrapper, Task>(taskFilePath));
            services.AddScoped<ITaskManager, TaskManager>();
            services.AddScoped<ITaskMapper, TaskMapper>();

            return services;
        }
        //Register FizzBuzzService
        public static IServiceCollection AddFizzBuzzServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFizzBuzzService, FizzBuzzService>();
            string? fizzBuzzFilePath = configuration.GetValue<string>("SharedJosn");
            services.AddScoped<ISharedRepository<FizzBuzzWrapper, FizzBuzzGamePlay>>(provider => new JsonSharedRepository<FizzBuzzWrapper, FizzBuzzGamePlay>(fizzBuzzFilePath));
            return services;
        }
        public static IServiceCollection AddUserManagementServices(this IServiceCollection services, IConfiguration configuration)
        { 
            string? usersFilePath = configuration.GetValue<string>("SharedJosn");

            services.AddScoped<ISharedRepository<UserWrapper, User>>(provider => new JsonSharedRepository<UserWrapper, User>(usersFilePath));
            services.AddScoped<IUserService, UserService>();
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
