using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETL.Client.Events
{
    public class LocalFileEventsProvider : IEventsProvider
    {
        private const string EventsFilePath = "../ETL.Files/events.txt";
        private readonly JsonSerializerOptions jsonOptions;

        public LocalFileEventsProvider()
        {
            jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            };
        }

        public async IAsyncEnumerable<LiveEvent> GetEvents()
        {
            if (!File.Exists(EventsFilePath))
            {
                throw new FileNotFoundException($"Events file not found at path: {EventsFilePath}");
            }
            else
            {
                // prefer readability over performance
                await foreach (var liveEvent in GetEventsCore())
                {
                    yield return liveEvent;
                }
            }
        }

        private async IAsyncEnumerable<LiveEvent> GetEventsCore()
        {
            using var stream = new FileStream(EventsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(stream);

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                LiveEvent? liveEvent = DeserializeEvent(line);
                if (liveEvent is not null)
                {
                    yield return liveEvent;
                }
            }
        }

        private LiveEvent? DeserializeEvent(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<LiveEvent>(json, jsonOptions);
            }
            catch
            {
                // Optionally log or handle malformed lines
                return null;
            }
        }
    }
}