 # C Sharp Projects for Beginners
This repository contains basic projects, including the popular FizzBuzz coding challenge and a Task Tracker CLI.

## FizzBuzz Console Arcade - Divisibility Challenge 🕹️
### Description
The FizzBuzz Game is a classic coding challenge that tests basic programming skills. The rules are simple:

Enter 5 numbers between 1 and 100. For each number:
- "FizzBuzz" if divisible by both 3 and 5 (10 points).
- "Fizz" if divisible by 3 (5 points).
- "Buzz" if divisible by 5 (5 points).
- Otherwise, 1 point.

### Preview of CLI
![Preview of FizzBuzz Game CLI](https://github.com/Dbrown127/csharp-projects-beg/blob/main/FizzBuzzConsole/images/fizzbuzz_game_preview-v1-03Sep2024.png?raw=true)

### Technical Notes 💻
- **Model Class (FizzBuzz):** Encapsulates game logic by determining "Fizz", "Buzz", "FizzBuzz", or "Number" with associated points.
- **Service Class (FizzBuzzService):** Handles core game logic, storing user inputs, and tallying points.
- **Interactive CLI:** Provides an engaging user experience, displaying rules and collecting validated inputs.
- **XML Documentation:** Comprehensive code comments using XML aid in understanding and maintaining the codebase.

- **OG Rules**:
Fizz Buzz Problem states that given an integer n, for every integer i <= n, the task is to print “FizzBuzz” if i is divisible by 3 and 5, “Fizz” if i is divisible by 3, “Buzz” if i is divisible by 5 or i (as a string) if none of the conditions are true.
![Diagram of FizzBuzz rules](https://media.geeksforgeeks.org/wp-content/uploads/20240110170933/fizz-buzz.png)

[Link to the project rules and implementation](https://www.geeksforgeeks.org/fizz-buzz-implementation/)

## FizzBuzz Arcade API 🎮
### Description
The **FizzBuzz Arcade API** extends the original FizzBuzz game into a web API format. The API exposes game functionality via HTTP endpoints, allowing external clients to interact with the FizzBuzz logic programmatically.

### Key Topics Discussed:
- **API for Games/Services**: Unlike CRUD-based APIs, this API focuses on providing computational services, maintaining game state, and handling operations like calculating FizzBuzz, counting results, and tallying points.
- **Model Binding and Validation**: Using data transfer objects (DTOs) and model binding to validate input sizes and constraints, ensuring only valid data is processed.
- **Dependency Injection and Service Registration**: Registering services with different lifetimes (`Singleton`, `Scoped`, etc.) to manage game state across API requests.
- **Mapping Data Objects**: Converting internal game data into API-friendly formats (DTOs) for easy consumption by clients.
- **Developing with SOLID Principles**: Following principles like Single Responsibility, Dependency Inversion, and Interface Segregation to keep the codebase modular and maintainable.

### Preview of API Endpoints
- `POST /api/FizzBuzz/values` - Save a list of FizzBuzz values.
- `DELETE /api/FizzBuzz/clear` - Clear previous results to reset the game state.
- `GET /api/FizzBuzz/count` - Get the count of Fizz, Buzz, and FizzBuzz.
- `GET /api/FizzBuzz/points` - Get the total points.
- `GET /api/FizzBuzz/values` - Get all saved values.

## Task Tracker Console 📊
Task Tracker is a project used to track and manage your tasks. This project helps practice programming skills, including working with the filesystem, handling user inputs, and building a simple CLI application.

### Requirements
The application should run from the command line, accept user actions and inputs as arguments, and store the tasks in a JSON file. The user should be able to:

- Add, Update, and Delete tasks
- Mark a task as `TODO`, `PENDING`, or `COMPLETE`
- List all tasks
- List all tasks that are complete
- List all tasks that are not done
- List all tasks that are in progress
[Link to project rules and examples](https://roadmap.sh/projects/task-tracker)
### Task Tracker API 📊
### Description
The **Task Tracker API** builds upon the CLI version, converting it into a RESTful API for managing tasks via HTTP requests. The API is designed around CRUD operations for managing task data.

### Key Topics Discussed:
- **API for Data Management**: This API focuses on CRUD operations (Create, Read, Update, Delete) for managing tasks, making it a typical data management API.
- **Model Binding and Validation**: Using DTOs and model binding to validate input data and ensure proper formatting.
- **Dependency Injection and Service Registration**: Leveraging DI to register services like `TaskManager` to handle task operations, maintaining separation of concerns.
- **Mapping Data Objects**: Converting between domain models and DTOs for consistent API responses.
- **Developing with SOLID Principles**: Emphasizing code maintainability and readability through SOLID principles like Open/Closed and Single Responsibility.

### Preview of API Endpoints
- `POST /api/TaskTracker/tasks` - Add a new task.
- `GET /api/TaskTracker/tasks` - List all tasks.
- `PUT /api/TaskTracker/tasks/{id}` - Update a task.
- `DELETE /api/TaskTracker/tasks/{id}` - Delete a task.

### Constraints
- The JSON file should be created if it does not exist.
- Use the native file system module of your programming language to interact with the JSON file.
- Do not use any external libraries or frameworks to build this project.
- Ensure to handle errors and edge cases gracefully.
