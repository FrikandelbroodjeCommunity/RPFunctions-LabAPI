using System;
using System.Linq;
using System.Threading.Tasks;
using CommandSystem;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles;
using RemoteAdmin;

namespace RPF.Commands.Client;

[CommandHandler(typeof(ClientCommandHandler))]
public class HackCommand : ICommand
{
    private static bool _isHackInProgress = false;
    private static bool _hackCompleted = false;

    public string Command => "hack";
    public string[] Aliases => new[] { ".hack" };
    public string Description => "Activate a timer of 60 seconds every time the hack command is executed";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (sender is PlayerCommandSender playerSender)
        {
            var player = Player.Get(playerSender.ReferenceHub);

            var roomName = player.CachedRoom.Name;
            if (roomName != RoomName.Hcz096)
            {
                response = "You can only do that in 096 room!";
                return false;
            }

            if (player.Team != Team.ChaosInsurgency)
            {
                response = "Only CI can execute this command!";
                return false;
            }

            if (!HasChaosKeycard(player))
            {
                response = "You must have at least a ChaosKeycard!";
                return false;
            }

            if (_hackCompleted)
            {
                response = "The command has already been executed!";
                return false;
            }

            if (_isHackInProgress)
            {
                response = "Your team is already executing this command!";
                return false;
            }

            _isHackInProgress = true;
            _ = RunHackAsync(player);
            response = "Hack in progress check the timer!.";
            return true;
        }

        response = "This command is only available for humans!";
        return false;
    }

    private static async Task RunHackAsync(Player player)
    {
        try
        {
            var targetRoom = player.CachedRoom.Name;

            for (var i = 60; i >= 0; i--)
            {
                if (ActiveGeneratorsCount() >= 3)
                {
                    player.SendHint("<color=red><b>[HACK]</b></color>\n" +
                                    "All generators have been activated Hack cancelled.", 10);
                    _isHackInProgress = false;
                    return;
                }

                if (player.CachedRoom == null || player.CachedRoom.Name != targetRoom)
                {
                    player.SendHint("<color=red><b>[HACK]</b></color>\n" +
                                    "You've left the room! The hack has been canceled. Try again...", 10);
                    _isHackInProgress = false;
                    return;
                }

                if (!HasChaosKeycard(player))
                {
                    player.SendHint("<color=red><b>[HACK]</b></color>\n" +
                                    "You no longer have the Chaos Insurgency keycard in your hand! The hack has been canceled.",
                        10);
                    _isHackInProgress = false;
                    return;
                }

                player.SendHint("<color=red><b>[HACK]</b></color>\n" +
                                $"Time remaining: {i}s", 2);
                await Task.Delay(1000);
            }

            _hackCompleted = true;

            Cassie.Message(
                "bell_start pitch_0.4 .G4 .G4 .G5 pitch_0.9 .G4 Error .G5 Error in CASSIESystem .G5 .G4 Activating pitch_0.2 .G4 pitch_0.9 security systems in T minus pitch_2  5 . 4 . 3 . 2 . 1 . pitch_0.2 .G4 .G3 pitch_0.8 Protocol failed .G4 Site is now under .G4 Delta Command control bell_end",
                isNoisy: false,
                customSubtitles:
                "<color=red>Error Error</color> in <color=blue>CASSIESystem</color><split>Activating <color=blue>security systems</color> in <color=red>T-5 ... 4 ... 3 ... 2 ... 1 ...</color><split><color=red> Protocol failed .G4</color><split><color=blue>Site</color> is now under .G4 <color=green>Delta Command</color> control!"
            );

            _ = FlickerLightsAsync();

            Cassie.Message(
                "The Chaos Insurgency has hacked the SCP Foundation's systems",
                isNoisy: false,
                customSubtitles: "The <color=green>Chaos Insurgency</color> has hacked the SCP Foundation's systems."
            );
        }
        catch (Exception ex)
        {
            Logger.Error($"[HackCommand] Error during timer: {ex}");
        }
        finally
        {
            _isHackInProgress = false;
        }
    }

    private static int ActiveGeneratorsCount()
    {
        return Generator.List.Count(g => g.Engaged);
    }

    private static bool HasChaosKeycard(Player player)
    {
        return player.Items.Any(x => x.Type == ItemType.KeycardChaosInsurgency);
    }

    private static async Task FlickerLightsAsync()
    {
        try
        {
            for (var i = 0; i < 5; i++)
            {
                Map.TurnOffLights(1f);
                await Task.Delay(1000);
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"[FlickerLights] Error: {ex}");
        }
    }

    public static void ResetState()
    {
        _isHackInProgress = false;
        _hackCompleted = false;
    }
}