using System.Linq;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace RPF.Events.RPSCP;

public static class Scp939DoorRestriction
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
        if (count < 2 || (count == 2 && Player.List.Any(x => x.Role == RoleTypeId.Scp096)))
        {
            return;
        }

        if (ev.Player.Role == RoleTypeId.Scp939)
        {
            ev.IsAllowed = false;
            ev.Player.SendHint(Main.Instance.Config.ScpRpFunctions939, 10);
        }
    }
}