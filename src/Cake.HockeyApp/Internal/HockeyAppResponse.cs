namespace Cake.HockeyApp.Internal
{
    using Newtonsoft.Json;

    internal class HockeyAppResponse
    {
        public string Version { get; set; }

        public string ShortVersion { get; set; }

        public string Title { get; set; }

        public string Id { get; set; }

        [JsonProperty("config_url")]
        public string ConfigUrl { get; set; }

        [JsonProperty("public_url")]
        public string PublicUrl { get; set; }

        public int? Status { get; set; }
    }
}