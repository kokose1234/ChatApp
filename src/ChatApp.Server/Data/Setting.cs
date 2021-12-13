using Newtonsoft.Json;

namespace ChatApp.Client.Data;

internal sealed record Setting
{
    [JsonIgnore]
    private static readonly Lazy<Setting> Lazy = new(JsonConvert.DeserializeObject<Setting>(File.ReadAllText("./ServerSetting.json")));

    [JsonIgnore]
    internal static Setting Value => Lazy.Value;

    [JsonProperty("host")]
    internal string Host { get; init; }

    [JsonProperty("username")]
    internal string Username { get; init; }

    [JsonProperty("password")]
    internal string Password { get; init; }

    [JsonProperty("port")]
    internal short Port { get; init; }
}