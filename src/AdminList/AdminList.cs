using System.Text.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace AdminList;

public abstract class AdminList : BasePlugin
{
    public override string ModuleName => "Admin List";
    public override string ModuleVersion => "1.0";
    public override string ModuleAuthor => "Quake1011";

    private static Config? _config;

    public override void Load(bool hotLoaded)
    {
        var configPath = Path.Join(ModuleDirectory, "Config.json");
        if (!File.Exists(configPath))
        {
            var data = new Config() { ImmunityFlag = "@css/root", ShowSelf = true, ShowFlag = "@css/ban" };
            File.WriteAllText(configPath, JsonSerializer.Serialize(data, new JsonSerializerOptions{ WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
            _config = data;
        }
        else _config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath));        
    }

    [ConsoleCommand("admins", "Prints admins list")]
    public void OnCommand(CCSPlayerController? activator, CommandInfo command)
    {
        var admins = 1;
        foreach (var player in Utilities.GetPlayers().Where(player => player is { IsBot: false, IsValid: true}).Where(player => _config?.ShowFlag != null && AdminManager.PlayerHasPermissions(player, _config.ShowFlag) && !AdminManager.PlayerHasPermissions(player, _config.ImmunityFlag!)))
        {
            if (player == activator) 
            {
                if (_config!.ShowSelf) activator.PrintToChat($"[#{admins}] {ChatColors.LightRed}{player.PlayerName}");
            }
            else activator?.PrintToChat($"[#{admins}] {ChatColors.LightRed}{player.PlayerName}");
            admins++;
        }
    }
    private class Config
     {
         public string? ImmunityFlag { get; init; }
         public bool ShowSelf { get; init; }
         public string? ShowFlag { get; init; }
     }
}


