using Microsoft.OpenApi.Models;
using System.Reflection;

namespace GemConnectAPI.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSwaggerGen(static opt =>
            {
                //Modify the information displayed in the UI
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GemConnectAPI - The Multi-Service Masterpiece",
                    Description = @"### Welcome to **GemConnectAPI**, your gateway to a unified and powerful multi-service platform!

This API seamlessly integrates multiple services like the **Task Tracker**, **FizzBuzzArcade**, and **Secure User Management**, bringing a treasure trove of functionality together under one gem of an API.

---

### Key Features:
- **Task Tracker**: Manage and track tasks efficiently with full CRUD (Create, Read, Update, Delete) operations.
- **FizzBuzzArcade**: Play the classic FizzBuzz game with a twist and track your performance.
- **Secure User Management**: Handle user registration, authentication, and profile management securely.
- **JWT-based Authorization**: Ensure robust security with role-based access control for both **ADMIN** and **USER** roles.

---

### How to Use:
1. **User Management**: Securely register, log in, and manage users through the `/api/auth` and `/api/user` endpoints.
2. **Task Tracker**: Organize your to-do lists and tasks using the `/api/tasktracker` endpoints.
3. **FizzBuzzArcade**: Play FizzBuzz using the `/api/fizzbuzz` endpoint, because who says coding can't be fun?

---

### Roles & Permissions:
- **Admins**: Can manage users and access advanced features.
- **Users**: Can manage their tasks and play FizzBuzz.

---

**GemConnectAPI** brings together the power of different services under one platform, ensuring smooth integration and ease of use for developers and users alike. Whether you're managing tasks, having fun with FizzBuzz, or securing user data, this API has you covered!",


                    Contact = new OpenApiContact
                {
                    Name = "GemConnectAPI Github Project",
                    Url = new Uri("https://github.com/Dbrown127/csharp-projects-beg")
                }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            return services;
        }
        private static string GetFizzBuzzSwaggerDescription() => @"Here's a fun and informative Swagger UI description for your **FizzBuzzArcade** API:\r\n\r\n---\r\n\r\n### **FizzBuzzArcade API**\r\n\r\nWelcome to the **FizzBuzzArcade API**, where the classic **FizzBuzz** challenge meets a modern twist! This API provides a playful and fun way to generate the classic FizzBuzz sequence while keeping score. Whether you're a developer looking to relive the FizzBuzz challenge or someone just seeking a little coding nostalgia, this arcade-style API has something for everyone.\r\n\r\n---\r\n\r\n### **Key Features:**\r\n- **FizzBuzz Generation**: Generate numbers and their FizzBuzz equivalent (Fizz for multiples of 3, Buzz for multiples of 5, FizzBuzz for both).\r\n- **Score Keeping**: Keep track of your progress by calculating points as you progress through the game.\r\n- **Customization**: Modify the range of numbers, or play the game with custom rules and challenges.\r\n\r\n---\r\n\r\n### **API Endpoints:**\r\n- **/api/fizzbuzz/{number}**: Returns the FizzBuzz value for a specific number.\r\n  - **Example Input**: `15`\r\n  - **Example Output**: `FizzBuzz`\r\n  \r\n- **/api/fizzbuzz/score**: Get your current score in the FizzBuzzArcade. Every correct FizzBuzz counts!\r\n\r\n---\r\n\r\n### **How to Get Started:**\r\n1. Use the `/api/fizzbuzz/{number}` endpoint to send a number and receive the corresponding FizzBuzz result.\r\n2. Play the game and track your score with the `/api/fizzbuzz/score` endpoint.\r\n3. Customize your experience with optional query parameters to change the rules or difficulty.\r\n\r\n---\r\n\r\n### **FizzBuzz Rules:**\r\n- **Fizz** for numbers divisible by **3**.\r\n- **Buzz** for numbers divisible by **5**.\r\n- **FizzBuzz** for numbers divisible by both **3** and **5**.\r\n- Regular numbers will be returned as-is if they don’t fit the FizzBuzz criteria.\r\n\r\n---\r\n\r\nHave fun playing with **FizzBuzzArcade**, whether you’re practicing for interviews, testing your coding skills, or just enjoying the game! 🎮✨\r\n\r\n---\r\n\r\nFeel free to tweak the description to better fit your app's style or any custom functionality you might have added to the **FizzBuzzArcade**. Let me know if you'd like to expand or adjust anything! 😄";
    }
}
