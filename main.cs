using System;
using System.Collections.Generic;

namespace AtBaGames
{
    /// <summary>
    /// AtBa SDK sample for C#.
    /// Version: 1.10.8
    /// Release Date: 2026-02-15
    /// </summary>
    public class AtBaSdk
    {
        public string Version { get; } = "1.10.8";
        public string ReleaseDate { get; } = "2026-02-15";
        public bool IsInitialized { get; private set; }
        public List<Game> Games { get; } = new List<Game>();
        public List<EventData> Events { get; } = new List<EventData>();

        public AtBaSdk()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (IsInitialized)
            {
                Console.WriteLine("AtBa SDK is already initialized.");
                return;
            }

            IsInitialized = true;
            Console.WriteLine($"AtBa Games SDK v{Version} initialized.");
        }

        public Game RegisterGame(string gameId, string name, string description = "", string url = "")
        {
            if (string.IsNullOrWhiteSpace(gameId))
                throw new ArgumentException("Game ID is required.", nameof(gameId));

            var game = new Game
            {
                Id = gameId,
                Name = string.IsNullOrWhiteSpace(name) ? gameId : name,
                Description = description,
                Url = url,
                RegisteredAt = DateTime.UtcNow
            };

            Games.Add(game);
            TrackEvent("game_registered", new Dictionary<string, object>
            {
                ["gameId"] = gameId,
                ["gameName"] = game.Name
            });

            return game;
        }

        public Game? GetGame(string gameId)
        {
            return Games.Find(g => g.Id == gameId);
        }

        public void TrackEvent(string eventName, Dictionary<string, object>? data = null)
        {
            if (string.IsNullOrWhiteSpace(eventName))
                throw new ArgumentException("Event name is required.", nameof(eventName));

            var eventData = new EventData
            {
                Name = eventName,
                Data = data ?? new Dictionary<string, object>(),
                Timestamp = DateTime.UtcNow
            };

            Events.Add(eventData);
            Console.WriteLine($"Event tracked: {eventName}");
        }

        public void Destroy()
        {
            IsInitialized = false;
            Games.Clear();
            Events.Clear();
            Console.WriteLine("AtBa Games SDK destroyed.");
        }
    }

    public class Game
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
    }

    public class EventData
    {
        public string Name { get; set; } = string.Empty;
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public DateTime Timestamp { get; set; }
    }

    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("AtBa Games C# SDK sample");

            var sdk = new AtBaSdk();
            var game = sdk.RegisterGame("rocket-racer", "Rocket Racer", "A fast arcade racing game.", "https://example.com/rocket-racer");

            Console.WriteLine($"Registered game: {game.Name} ({game.Id})");

            sdk.TrackEvent("game_started", new Dictionary<string, object>
            {
                ["gameId"] = game.Id,
                ["session"] = Guid.NewGuid().ToString()
            });

            Console.WriteLine($"Total tracked events: {sdk.Events.Count}");
        }
    }
}
