using LabApi.Events.Arguments.Scp914Events;
using LabApi.Events.Handlers;

namespace RPF.Events._914Event;

public static class Kill914
{
    public static void RegisterEvents()
    {
        Scp914Events.ProcessedPlayer += OnPlayerUpgrade;
    }

    public static void UnregisterEvents()
    {
        Scp914Events.ProcessedPlayer -= OnPlayerUpgrade;
    }
    
    private static void OnPlayerUpgrade(Scp914ProcessedPlayerEventArgs ev)
    {
        ev.Player.Kill("Corroded by SCP-914.");
    }
}