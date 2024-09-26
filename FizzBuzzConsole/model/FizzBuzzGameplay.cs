using System.Text.Json.Serialization;
using System.Text.Json;
using FizzBuzzConsole.manager;

namespace FizzBuzzConsole.model
{
    /// <summary>
    /// Represents a FizzBuzz gameplay session, including guesses and total points.
    /// </summary>
    public class FizzBuzzGamePlay
    {
        private readonly IFizzBuzzManager manager = new FizzBuzzManager();

       
        /// <summary>
        /// Gets or sets the ID of the gameplay session.
        /// </summary>
        [JsonPropertyName("game_play_id")]
        public int GamePlayId { get; set; }

        /// <summary>
        /// Gets or sets the player associated with the gameplay session.
        /// </summary>
        [JsonPropertyName("player")]
        public int Player { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of guesses made during the gameplay session, where
        /// the key is the number guessed, and the value is the corresponding FizzBuzz result.
        /// </summary>
        [JsonPropertyName("guesses")]
        [JsonConverter(typeof(FizzBuzzGuessConverter))]
        public Dictionary<int, FizzBuzzGuess> Guesses { get; set; } = [];

        /// <summary>
        /// Gets or sets the total points earned during the gameplay session.
        /// </summary>
        [JsonPropertyName("total_points")]
        public int TotalPoints { get; set; }

        /// <summary>
        /// Adds a guess and updates the total points for the gameplay session.
        /// </summary>
        /// <param name="number">The number guessed.</param>
        /// <param name="guess">The FizzBuzz result (e.g., Fizz, Buzz, FizzBuzz, or None).</param>
        public void AddGuess(int number, FizzBuzzGuess guess)
        {
            Guesses[number] = guess;
            TotalPoints += manager.CalculatePoints(guess);
        }
    }
    public class FizzBuzzWrapper
    {
        [JsonPropertyName("fizz_buzz_games")]
        public List<FizzBuzzGamePlay>? FizzBuzzGames { get; set; } = [];
    }

    /// <summary>
    /// Converts FizzBuzzGuess values to and from JSON.
    /// </summary>
    public class FizzBuzzGuessConverter : JsonConverter<Dictionary<int, FizzBuzzGuess>>
    {
        public override Dictionary<int, FizzBuzzGuess> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dictionary = new Dictionary<int, FizzBuzzGuess>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                var key = int.Parse(reader.GetString()!);

                reader.Read();
                var value = Enum.Parse<FizzBuzzGuess>(reader.GetString()!, true);

                dictionary[key] = value;
            }

            return dictionary;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<int, FizzBuzzGuess> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var kvp in value)
            {
                writer.WritePropertyName(kvp.Key.ToString());
                writer.WriteStringValue(kvp.Value.ToString());
            }
            writer.WriteEndObject();
        }
    }

}
