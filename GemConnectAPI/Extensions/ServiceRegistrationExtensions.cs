using FizzBuzzConsole.service;
using GemConnectAPI.Mappers.SecureUser;
using GemConnectAPI.Mappers.TaskTracker;
using GemConnectAPI.Services.SecureUser;
using SecureUserConsole.data;
using SecureUserConsole.service;
using TaskTrackerConsole.data;

namespace GemConnectAPI.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddTaskTrackerServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Task Tracker repository and manager
            string? taskFilePath = configuration.GetValue<string>("TaskFilePath");
            services.AddScoped<ITaskRepository>(provider => new TaskRepository(taskFilePath));
            services.AddScoped<ITaskManager, TaskManager>();
            services.AddScoped<ITaskMapper, TaskMapper>();

            return services;
        }
        //Register FizzBuzzService
        public static IServiceCollection AddFizzBuzzServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IFizzBuzzService, FizzBuzzService>();
            return services;
        }
        public static IServiceCollection AddUserManagementServices(this IServiceCollection services, IConfiguration configuration)
        {
            string? usersFilePath = configuration.GetValue<string>("UsersFilePath");

            services.AddScoped<IUserRepository>(provider => new UserRepository(usersFilePath));
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
