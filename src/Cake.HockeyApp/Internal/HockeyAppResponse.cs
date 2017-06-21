namespace Cake.HockeyApp.Internal
{
    using Newtonsoft.Json;

    internal class HockeyAppResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string Version { get; set; }

        public string ShortVersion { get; set; }

        public string Title { get; set; }

        public string Id { get; set; }

        [JsonProperty("config_url")]
        public string ConfigUrl { get; set; }

        [JsonProperty("public_url")]
        public string PublicUrl { get; set; }

        [JsonProperty("status")]
        public string StatusRaw { get; set; }

        [JsonIgnore]
        public int? Status
        {
            get
            {
                int num;
                return int.TryParse(StatusRaw, out num) ? (int?) num : null;
            }
        }
    }
}