using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrikanUtils.ProjectMer;
using Interactables.Interobjects.DoorUtils;
using LabApi.Events.Handlers;
using LabApi.Features.Enums;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using ProjectMER.Features.Extensions;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace RPF.Events.Misc;

public static class FemurBreakerEvent
{
    private static Config Config => Main.Instance.Config;

    private static bool _isRunning;
    private static bool _doorUnlockedByGenerators;
    private static Door _entranceDoor;
    private static Door _chamberDoor;
    private static CoroutineHandle _handle;

    private static FemurBreakerKillTrigger _trigger;

    public static void RegisterEvents()
    {
        ServerEvents.RoundStarted += OnRoundStarted;
    }

    public static void UnregisterEvents()
    {
        ServerEvents.RoundStarted -= OnRoundStarted;

        if (_handle.IsRunning)
        {
            Timing.KillCoroutines(_handle);
        }
    }

    private static void OnRoundStarted()
    {
        if (!Assets.Loaded) return;

        var room = Room.Get(RoomName.Hcz106).First();
        var spawned = Assets.FemurBreaker.SpawnSchematic(room.Position, room.GetAbsoluteRotation(Vector3.zero));

        _trigger = spawned.FindNamedObjects("Trigger").First().gameObject.AddComponent<FemurBreakerKillTrigger>();
        _trigger.Door = spawned.FindNamedObjects("Door").First();
        _trigger.OriginalPos = _trigger.Door.position;
        _trigger.Door.position = Vector3.zero;

        spawned.FindNamedObjects("Button").First().RegisterPickupAction((ply, _) =>
        {
            if (_isRunning || !_trigger.HasSacrifice) return;
            if (ply.IsHuman || !Config.OnlyHumansCanTrigger)
            {
                Timing.RunCoroutine(RunFemurBreaker());
            }
        }, "FemurBreaker", 10);

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
            _chamberDoor.Lock(DoorLockReason.AdminCommand, true);
        }

        if (_handle.IsRunning)
        {
            Timing.KillCoroutines(_handle);
        }

        _handle = Timing.RunCoroutine(MonitorGeneratorsAsync());
    }

    private static IEnumerator<float> MonitorGeneratorsAsync()
    {
        yield return Timing.WaitForSeconds(60);
        while (Round.IsRoundStarted)
        {
            var active = CountActiveGenerators();
            if (!_doorUnlockedByGenerators && active >= Config.GeneratorsRequired)
            {
                _doorUnlockedByGenerators = true;

                _entranceDoor?.Lock(DoorLockReason.AdminCommand, false);
                _chamberDoor?.Lock(DoorLockReason.AdminCommand, false);

                Logger.Info("[FemurBreaker] Generators threshold reached: SCP-106 doors unlocked.");
                yield break;
            }

            yield return Timing.WaitForSeconds(5);
        }
    }

    private static int CountActiveGenerators()
    {
        return Generator.List.Count(g => g.Engaged);
    }

    public static IEnumerator<float> RunFemurBreaker()
    {
        _isRunning = true;

        Map.SetColorOfLights(Color.red);
        Cassie.Message(
            "ACTIVATING FEMUR BREAKER",
            isNoisy: false,
            customSubtitles: "Activating femur breaker");

        yield return Timing.WaitForSeconds(Config.FemurBreakerDelaySeconds);

        var scp106 = Player.List.FirstOrDefault(p => p.IsAlive && p.Role == RoleTypeId.Scp106);
        if (scp106 != null)
        {
            scp106.Damage(new UniversalDamageHandler(scp106.Health * 2, DeathTranslations.Recontained));
            Logger.Info("[FemurBreaker] SCP-106 neutralized.");
        }

        Cassie.Message(
            "SCP 1 0 6 SUCCESSFULLY TERMINATED",
            isNoisy: false,
            customSubtitles: "SCP-106 successfully terminated");
        Map.SetColorOfLights(Color.green);
        yield return Timing.WaitForSeconds(1);
        Map.ResetColorOfLights();
    }
}