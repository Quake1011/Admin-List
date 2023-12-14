using System.Text.Json.Serialization;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace AdminList;

public class SampleConfig : BasePluginConfig
{
    [JsonPropertyName("ImmunityFlag")] public string ImmunityFlag { get; set; } = "@css/root";

    [JsonPropertyName("ShowSelf")] public bool ShowSelf { get; set; } = true;

    [JsonPropertyName("ShowFlag")] public string ShowFlag { get; set; } = "@css/ban";
}

[MinimumApiVersion(126)]
public class AdminList : BasePlugin, IPluginConfig<SampleConfig>
{
    public override string ModuleName => "Admin List";
    public override string ModuleVersion => "1.0";
    public override string ModuleAuthor => "Quake1011";
    public SampleConfig Config { get; set; } = null!;
    public void OnConfigParsed(SampleConfig config)
    {
        Config = config;
    }

    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    [ConsoleCommand("admins", "Prints admins list")]
    public void OnCommand(CCSPlayerController? activator, CommandInfo command)
    {
        var admins = 1;
        activator?.PrintToChat($" {ChatColors.Red}Admins {ChatColors.Green}Online{ChatColors.Red}:");
        activator?.PrintToChat($" {ChatColors.Red}------------------------");
        foreach (var player in Utilities.GetPlayers().Where(player => player is { IsBot: false, IsValid: true }).Where(player => AdminManager.PlayerHasPermissions(player, Config.ShowFlag) && !AdminManager.PlayerHasPermissions(player, Config.ImmunityFlag)))
        {
            if (player == activator)
            {
                if (!Config.ShowSelf) continue;
                activator.PrintToChat($" [#{admins}] {ChatColors.LightRed}{player.PlayerName}");
                admins++;
            }
            else
            {
                activator?.PrintToChat($" [#{admins}] {ChatColors.LightRed}{player.PlayerName}");
                admins++;
            }
        }

        activator?.PrintToChat($" {ChatColors.Red}------------------------");
        if (admins == 1) activator?.PrintToChat($" {ChatColors.Red}At the moment there are no admins on the server");
    }
}

