using System;
using Interactables.Interobjects.DoorUtils;
using LabApi.Events.Handlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using MEC;

namespace RPF.Events.BroadCast;

public static class BroadCastBreach
{
    public static void RegisterEvents()
    {
        ServerEvents.RoundStarted += OnRoundStarted;
    }

    public static void UnregisterEvents()
    {
        ServerEvents.RoundStarted -= OnRoundStarted;
    }

    private static void OnRoundStarted()
    {
        if (Main.Instance.Config.StartAnnoucment != true) return;
        FlickerAllLights();
    }
    
    private static void FlickerAllLights()
    {
        try
        {
            Map.TurnOffLights(10f);
            foreach (var door in Door.List)
            {
                door.Lock(DoorLockReason.Lockdown079, true);
            }

            Timing.CallDelayed(10, () =>
            {
                foreach (var door in Door.List)
                {
                    door.Lock(DoorLockReason.Lockdown079, false);
                }
            });
            
            Cassie.Message("bell_start pitch_0.4 .G4 .G4 .G5 pitch_0.9 .G4 Attention .G3 Attention .G4 SCP ? ? ? has not escaped out of the containment pitch_0.4 .G4 pitch_0.9 repeat .G4 SCP ? ? ? has not breached the containment bell_end",
                isNoisy: false,
                customSubtitles: "<color=blue>Attention Attention</color> <color=red>SCP-???</color> has not escaped out of the <color=red>containment</color> repeat <color=red>SCP-???</color> has not <color=red>breached</color> the <color=red>containment</color>");
        }
        catch (Exception ex)
        {
            Logger.Error($"[FlickerLights] Error: {ex}");
        }
    }
}