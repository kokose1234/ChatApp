using Newtonsoft.Json;

namespace ChatApp.Client.Data;

internal sealed record Setting
{
    [JsonIgnore]
    private static readonly Lazy<Setting> Lazy = new(JsonConvert.DeserializeObject<Setting>(File.ReadAllText("./ServerSetting.json")) ?? new());

    [JsonIgnore]
    internal static Setting Value => Lazy.Value;

    [JsonProperty("host")]
    internal string Host { get; init; } = "127.0.0.1";

    [JsonProperty("username")]
    internal string Username { get; init; } = "root";

    [JsonProperty("password")]
    internal string Password { get; init; } = "root";

    [JsonProperty("port")]
    internal short Port { get; init; } = 3306;
}