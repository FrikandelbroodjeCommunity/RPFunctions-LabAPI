using System;
using System.Threading.Tasks;
using CommandSystem;
using LabApi.Features.Wrappers;
using PlayerRoles;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace RPF.Commands.Client;

[CommandHandler(typeof(ClientCommandHandler))]
public class Overload : ICommand
{
    public string Command => "overload";
    public string[] Aliases => Array.Empty<string>();
    public string Description => "Command for 079";
    
    public static bool UsedThisRound;
        
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender);

        if (player.Role != RoleTypeId.Scp079)
        {
            response = "You must be Scp 079";
            return false;
        }

        if (UsedThisRound)
        {
            response = "You cannot do this command anymore you have already done it!";
            return false;
        }

        UsedThisRound = true;

        response = "Overload in progress...";
        Task.Run(async () =>
        {
            await FlickerLights();
            await LightsColor();
        });

        return true;
    }

    private static async Task FlickerLights()
    {
        if (Main.Instance.Config.EnableOverloadCommand != true) return;
        try
        {
            Map.TurnOffLights(5f);
            await Task.Delay(500);
        }
        catch (Exception ex)
        {
            Logger.Error($"[FlickerLights] error in executing command: {ex}");
        }
    }

    private static async Task LightsColor()
    {
        if (Main.Instance.Config.EnableOverloadCommand != true) return;
            
        try
        {
            Map.SetColorOfLights(Color.red);
            await Task.Delay(500);
            Cassie.Message(
                Main.Instance.Config.Overload079Cassie,
                isNoisy: false,
                isSubtitles: true
            );
        }
        catch (Exception ex)
        {
            Logger.Error($"[DoorLocksColor] error in executing command: {ex}");
        }
    }
}