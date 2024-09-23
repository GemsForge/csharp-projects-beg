using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskTrackerConsole.model
{
    public class StatusEnumConverter : JsonConverter<Status>
    {
        public override Status Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string enumString = reader.GetString();
            // Try to parse the enum ignoring case
            if (Enum.TryParse<Status>(enumString, true, out var result))
            {
                return result;
            }
            throw new JsonException($"Unable to convert \"{enumString}\" to {nameof(Status)}.");
        }

        public override void Write(Utf8JsonWriter writer, Status value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
