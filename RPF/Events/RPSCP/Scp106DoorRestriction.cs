using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace RPF.Events.RPSCP;

public static class Scp106DoorRestriction
{
    public static void RegisterEvents()
    {
        PlayerEvents.InteractingDoor += OnInteractingDoor;
    }

    public static void UnregisterEvents()
    {
        PlayerEvents.InteractingDoor -= OnInteractingDoor;
    }

    private static void OnInteractingDoor(PlayerInteractingDoorEventArgs ev)
    {
        if (ev.Player.Role != RoleTypeId.Scp106 || ev.Door is ElevatorDoor)
        {
            return;
        }

        ev.IsAllowed = false;
        ev.Player.SendHint(Main.Instance.Config.ScpRpFunctions106, 10);
    }
}