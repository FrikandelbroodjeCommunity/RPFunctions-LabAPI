using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
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
        if (ev.Player.Role == RoleTypeId.Scp939)
        {
            ev.IsAllowed = false;
            ev.Player.SendHint(Main.Instance.Config.ScpRpFunctions939, 10);
        }
    }
}