using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interactables.Interobjects.DoorUtils;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Enums;
using LabApi.Features.Wrappers;
using PlayerRoles;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace RPF.Events.Misc;

public static class FemurBreakerEvent
{
    private static readonly Config _config;

    private static bool _isRunning;
    private static bool _doorUnlockedByGenerators;
    private static Door _entranceDoor;
    private static Door _chamberDoor;
    private static CancellationTokenSource _monitorCts;

    public static void RegisterEvents()
    {
        PlayerEvents.InteractingDoor += OnDoorInteract;
        ServerEvents.RoundStarted += OnRoundStarted;
    }

    public static void UnregisterEvents()
    {
        PlayerEvents.InteractingDoor -= OnDoorInteract;
        ServerEvents.RoundStarted -= OnRoundStarted;

        _monitorCts?.Cancel();
        _monitorCts?.Dispose();
        _monitorCts = null;
    }

    private static void OnRoundStarted()
    {
        _isRunning = false;
        _doorUnlockedByGenerators = false;

        _entranceDoor = Door.List.FirstOrDefault(d => d.DoorName == DoorName.Hcz106Primiary);
        _chamberDoor = Door.List.FirstOrDefault(d => d.DoorName == DoorName.Hcz106Secondary);

        if (_entranceDoor != null)
        {
            _entranceDoor.IsOpened = false;
            _entranceDoor.Lock(DoorLockReason.AdminCommand, true);
        }

        if (_chamberDoor != null)
        {
            _chamberDoor.IsOpened = false;
            _entranceDoor.Lock(DoorLockReason.AdminCommand, true);
        }

        _monitorCts?.Cancel();
        _monitorCts?.Dispose();
        _monitorCts = new CancellationTokenSource();
        _ = MonitorGeneratorsAsync(_monitorCts.Token);
    }

    private static void OnDoorInteract(PlayerInteractingDoorEventArgs ev)
    {
        if (ev.Door != _entranceDoor && ev.Door != _chamberDoor)
        {
            return;
        }

        if (!_doorUnlockedByGenerators && CountActiveGenerators() < _config.GeneratorsRequired)
        {
            ev.Player.SendHint("<color=red>The cell is Blocked! Activate all Generators!</color>", 10);
            ev.IsAllowed = false;
            return;
        }

        if (_config.OnlyHumansCanTrigger && (ev.Player.Team == Team.SCPs || ev.Player.Team == Team.Dead))
        {
            return;
        }

        if (!_isRunning)
        {
            _ = RunFemurBreaker();
        }
    }

    private static async Task MonitorGeneratorsAsync(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested && Round.IsRoundStarted)
            {
                var active = CountActiveGenerators();
                if (!_doorUnlockedByGenerators && active >= _config.GeneratorsRequired)
                {
                    _doorUnlockedByGenerators = true;

                    _entranceDoor?.Lock(DoorLockReason.AdminCommand, false);
                    _chamberDoor?.Lock(DoorLockReason.AdminCommand, false);

                    Logger.Info("[FemurBreaker] Generators threshold reached: SCP-106 doors unlocked.");
                    break;
                }

                await Task.Delay(1000, ct);
            }
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            Logger.Error($"[FemurBreaker] MonitorGeneratorsAsync error: {ex}");
        }
    }

    private static int CountActiveGenerators()
    {
        return Generator.List.Count(g => g.Engaged);
    }

    public static async Task RunFemurBreaker()
    {
        Logger.Info("[FemurBreaker] Activated: playing ambient sound.");

        Map.SetColorOfLights(Color.red);
        Cassie.Message(
            "ACTIVATING FEMUR BREAKER",
            isNoisy: false,
            customSubtitles: "Activating femur breaker");

        await Task.Delay(_config.FemurBreakerDelay);

        var scp106 = Player.List.FirstOrDefault(p => p.IsAlive && p.Role == RoleTypeId.Scp106);
        if (scp106 != null)
        {
            scp106.Kill("Killed by FemurBreaker");
            Logger.Info("[FemurBreaker] SCP-106 neutralized.");
        }

        Cassie.Message(
            "SCP 1 0 6 SUCCESSFULLY TERMINATED",
            isNoisy: false,
            customSubtitles: "SCP-106 successfully terminated");
        Map.SetColorOfLights(Color.green);
        await Task.Delay(1000);
        Map.ResetColorOfLights();
    }
}