using Newtonsoft.Json;

namespace ChatApp.Client.Data;

internal sealed record Setting
{
    [JsonIgnore]
    private static readonly Lazy<Setting> Lazy = new(JsonConvert.DeserializeObject<Setting>(File.ReadAllText("./Setting.json")));

    [JsonIgnore]
    internal static Setting Value => Lazy.Value;

    [JsonProperty("serverAddress")]
    public string ServerAddress { get; init; }

    [JsonProperty("serverPort")]
    public int ServerPort { get; init; }
}