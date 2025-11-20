using System;
using CommandSystem;
using LabApi.Features.Wrappers;
using MapGeneration;
using RPF.Events.Misc;

namespace RPF.Commands.Client;

[CommandHandler(typeof(ClientCommandHandler))]
public class FemurActivator : ICommand
{
    public string Command { get; } = Main.Instance.Config.FemurCommand;
    public string[] Aliases { get; } = { "femur" };
    public string Description => "Activate Femur Event.";
    
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender);

        var roomName = player.CachedRoom.Name;
        if (roomName != RoomName.Hcz106)
        {
            response = "You can only do that in 106 room!";
            return false;
        }

        response = "Running Femur Breaker...";
        _ = FemurBreakerEvent.RunFemurBreaker();
        return true;
    }
}