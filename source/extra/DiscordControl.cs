using System.Diagnostics;
using DiscordRPC;
using DiscordRPC.Logging;

namespace Celestia.Extra.Discord {
    public static class DiscordControl {
        private static DiscordRpcClient _client;

        public static void Init(string clientId) {
            _client = new DiscordRpcClient(clientId);

            _client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            _client.OnPresenceUpdate += (sender, e) => { Debug.WriteLine("Presence updated: {0}", e.Presence); };

            _client.Initialize();

            _client.SetPresence(new RichPresence() {
                Details = "Trying this out.",
                State = "Saying hello."
            });
        }

    }
}