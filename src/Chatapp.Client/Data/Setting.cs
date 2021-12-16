using Newtonsoft.Json;
using System;
using System.IO;

namespace ChatApp.Client.Data;

internal sealed record Setting
{
    [JsonIgnore]
    private static readonly Lazy<Setting> Lazy = new(JsonConvert.DeserializeObject<Setting>(File.ReadAllText("./ClientSetting.json")) ?? new());

    [JsonIgnore]
    internal static Setting Value => Lazy.Value;

    [JsonProperty("serverAddress")]
    public string ServerAddress { get; init; } = "127.0.0.1";

    [JsonProperty("serverPort")]
    public int ServerPort { get; init; } = 12000;
}