using System;
using CommandSystem;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace RPF.Commands.Client;

[CommandHandler(typeof(ClientCommandHandler))]
public class Scientist : ICommand
{
    public string Command => "escapeTool";
    public string[] Aliases => new[] { "escape" };
    public string Description => "A Scientist can escape the facility";
        
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender);

        if (player.Role != RoleTypeId.Scientist)
        {
            response = "You MUST be a Scientist to run this command!";
            return false;
        }
            
        if (_usedThisRound)
        {
            response = "You can't do the command anymore, another Scientist Already executed it!";
            return false;
        }
            
        _usedThisRound = true;

        response = "Command received.";
        player.SendHint(Main.Instance.Config.ScientistInstructions, 10);
        player.AddItem(ItemType.KeycardFacilityManager);
        return true;
    }

    private static bool _usedThisRound = false;
}