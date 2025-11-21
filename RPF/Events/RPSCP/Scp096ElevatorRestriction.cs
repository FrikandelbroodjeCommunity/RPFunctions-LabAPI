using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp096;

namespace RPF.Events.RPSCP;

public static class Scp096ElevatorRestriction
{
    public static void RegisterEvents()
    {
        PlayerEvents.InteractingElevator += OnInteractingElevator;
    }

    public static void UnregisterEvents()
    {
        PlayerEvents.InteractingElevator -= OnInteractingElevator;
    }

    private static void OnInteractingElevator(PlayerInteractingElevatorEventArgs ev)
    {
        var count = Player.List.Count(x => x.Team == Team.SCPs);
        if (count < 2 || (count == 2 && Player.List.Any(x => x.Role == RoleTypeId.Scp939)))
        {
            return;
        }
        
        var player = ev.Player;
        if (player.Role != RoleTypeId.Scp096 || player.RoleBase is not Scp096Role role ||
            role.IsRageState(Scp096RageState.Enraged))
        {
            return;
        }

        ev.IsAllowed = false;
        player.SendHint(Main.Instance.Config.ScpRpFunctions096, 10);
    }
}