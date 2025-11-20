using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using PlayerRoles;

namespace RPF.Events.TeslaGate;

public class TeslaConditions
{
    public static void RegisterEvents()
    {
        PlayerEvents.TriggeringTesla += OnTeslaActivated;
    }

    public static void UnregisterEvents()
    {
        PlayerEvents.TriggeringTesla -= OnTeslaActivated;
    }

    private static void OnTeslaActivated(PlayerTriggeringTeslaEventArgs ev)
    {
        if (ev.Player.Team == Team.FoundationForces)
        {
            ev.IsAllowed = false;
        }
    }
}