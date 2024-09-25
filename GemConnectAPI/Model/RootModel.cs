using FizzBuzzConsole.model;
using SecureUserConsole.model;
using System.Text.Json.Serialization;
using Task = TaskTrackerConsole.model.Task;

namespace GemConnectAPI.Model
{
    /// <summary>
    /// Represents the root model containing users, tasks, and FizzBuzz games.
    /// </summary>
    public class RootModel
    {
        /// <summary>
        /// A collection of users.
        /// </summary>
        [JsonPropertyName("users")]
        public required List<User> Users { get; set; }

        /// <summary>
        /// A collection of tasks.
        /// </summary>
        [JsonPropertyName("tasks")]
        public required List<Task> Tasks { get; set; }

        /// <summary>
        /// A collection of FizzBuzz gameplays.
        /// </summary>
        [JsonPropertyName("fizz_buzz_games")]
        public required List<FizzBuzzGamePlay> FizzBuzzGames { get; set; }
    }
}
