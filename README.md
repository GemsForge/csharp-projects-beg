# C Sharp Projects for Begginners
This repository contains basic projects including the popular FizzBuzz coding challenge.

## FizzBuzz Game - Divisibility Challange 🕹️
### Description
The FizzBuzz Game is a classic coding challenge that tests basic programming skills. The rules are simple:

Enter 5 numbers between 1 and 100.
For each number:
- "FizzBuzz" if divisible by both 3 and 5 (10 points).
- "Fizz" if divisible by 3 (5 points).
- "Buzz" if divisible by 5 (5 points).
- Otherwise, 1 point.

### Preview of CLI
![Preiew of FizzBuzz Game CLI](https://github.com/Dbrown127/csharp-projects-beg/blob/fizzbuzz/FizzBuzzGame/images/fizzbuzz_game_preview-v1-03Sep2024.png)

### Tehnical Notes 💻
- **Model Class (FizzBuzz):** Encapsulates game logic by determining "Fizz", "Buzz", "FizzBuzz", or "Number" with associated points.
- **Service Class (FizzBuzzService):** Handles core game logic, storing user inputs, and tallying points.
- **Interactive CLI:** Provides an engaging user experience, displaying rules and collecting validated inputs.
- **XML Documentation:** Comprehensive code comments using XML, aiding in understanding and maintaining the codebase.

- ** OG Rules**:
Fizz Buzz Problem states that given an integer n, for every integer i <= n, the task is to print “FizzBuzz” if i is divisible by 3 and 5, “Fizz” if i is divisible by 3, “Buzz” if i is divisible by 5 or i (as a string) if none of the conditions are true.
![Diagram of FizzBuzz rules](https://media.geeksforgeeks.org/wp-content/uploads/20240110170933/fizz-buzz.png)

[Link to the project rules and implementation](https://www.geeksforgeeks.org/fizz-buzz-implementation/)

## Task Tracker
Task tracker is a project used to track and manage your tasks. In this task, you will build a simple command line interface (CLI) to track what you need to do, what you have done, and what you are currently working on. This project will help you practice your programming skills, including working with the filesystem, handling user inputs, and building a simple CLI application.

**Requirements**
The application should run from the command line, accept user actions and inputs as arguments, and store the tasks in a JSON file. The user should be able to:

- Add, Update, and Delete tasks
- Mark a task as  `TODO`, `PENDING` or `COMPLETE`
- List all tasks
- List all tasks that are complete
- List all tasks that are not done
- List all tasks that are in progress

Here are some constraints to guide the implementation:

- You can use any programming language to build this project.
- Use positional arguments in command line to accept user inputs.
- Use a JSON file to store the tasks in the current directory.
- The JSON file should be created if it does not exist.
- Use the native file system module of your programming language to interact with the JSON file.
- Do not use any external libraries or frameworks to build this project.
- Ensure to handle errors and edge cases gracefully.

[Link to project rules and examples](https://roadmap.sh/projects/task-tracker)
