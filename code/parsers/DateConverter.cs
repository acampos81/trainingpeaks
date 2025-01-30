using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace trainingpeaks
{
	/// <summary>
	/// Custom JSON DateTime parser for format "yyyy-MM-dd HH:mm:ss"
	/// </summary>
	public class DateConverter : JsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
		{
			string? dateStr = reader.GetString();
			return DateTime.Parse(string.IsNullOrEmpty(dateStr) ? string.Empty : dateStr);
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString("yyyy'-'MM'-'dd HH':'mm':'ss"));
		}
	}
}
