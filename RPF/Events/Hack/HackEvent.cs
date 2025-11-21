using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrikanUtils.ProjectMer;
using FrikanUtils.Utilities;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using PlayerRoles;
using ProjectMER.Features.Extensions;
using UnityEngine;

namespace RPF.Events.Hack;

public static class HackEvent
{
    private static bool _isHackInProgress = false;
    private static bool _hackCompleted = false;

    private static TextToy Text;

    public static void RegisterEvents()
    {
        ServerEvents.RoundStarted += OnRoundStart;
    }

    public static void UnregisterEvents()
    {
        ServerEvents.RoundStarted -= OnRoundStart;
    }

    private static void OnRoundStart()
    {
        if (!Assets.Loaded) return;

        var room = Room.Get(RoomName.Hcz096).First();
        var spawned = Assets.HackDevice.SpawnSchematic(room.Position, room.GetAbsoluteRotation(Vector3.zero));
        Text = TextToy.Get(spawned.FindNamedObjects("Text").First().GetComponent<AdminToys.TextToy>());
        Text.Scale = Vector3.one * 0.1f;

        ResetText();

        spawned.FindNamedObjects("Interactable").First()
            .RegisterPickupAction((ply, _) => { StartHack(ply); }, "hackdevice", 2);
    }

    private static void StartHack(Player player)
    {
        if (_hackCompleted)
        {
            player.SendHint("The hack has already been executed", 5);
            return;
        }

        if (_isHackInProgress)
        {
            player.SendHint("A hack is already in progress", 5);
        }

        if (player.Team != Team.ChaosInsurgency)
        {
            player.SendHint("Only Chaos Insurgency can hack", 5);
            return;
        }

        if (player.Items.All(x => x.Type != ItemType.KeycardChaosInsurgency))
        {
            player.SendHint("You must have at least a ChaosKeycard", 5);
            return;
        }

        Timing.RunCoroutine(RunHack(player));
    }

    private static IEnumerator<float> RunHack(Player player)
    {
        player.SendHint("You have started the hack\nStay in 096 containment until the hack is complete", 10);

        for (var i = 60; i >= 0; i--)
        {
            var builder = new StringBuilder($"<color=green><b>Hack in progress</b></color>\nTime remaining: {i}s\n\n[");
            for (var j = 0; j < 60; j += 5)
            {
                builder.Append(j < 60 - i ? "=" : "-");
            }

            builder.Append("]");
            Text.TextFormat = builder.ToString();

            if (Generator.List.Count(x => x.Engaged) >= 3)
            {
                player.SendHint("<color=red><b>[HACK]</b></color>\n" +
                                "All generators have been activated Hack cancelled.", 10);
                _isHackInProgress = false;
                ResetText();
                yield break;
            }

            if (player.CachedRoom == null || player.CachedRoom.Name != RoomName.Hcz096)
            {
                player.SendHint("<color=red><b>[HACK]</b></color>\n" +
                                "You've left the room! The hack has been canceled. Try again...", 10);
                _isHackInProgress = false;
                ResetText();
                yield break;
            }

            if (player.Items.All(x => x.Type != ItemType.KeycardChaosInsurgency))
            {
                player.SendHint("<color=red><b>[HACK]</b></color>\n" +
                                "You no longer have the Chaos Insurgency keycard! The hack has been canceled.",
                    10);
                _isHackInProgress = false;
                ResetText();
                yield break;
            }

            player.SendHint("<color=red><b>[HACK]</b></color>\n" +
                            $"Time remaining: {i}s\n\n" +
                            "Stay in 096 containment until the hack is complete", 2);

            yield return Timing.WaitForSeconds(1);
        }

        _hackCompleted = true;
        Text.TextFormat = "<color=red>Hack has been completed</color>";

        Cassie.Message(
            "bell_start pitch_0.4 .G4 .G4 .G5 pitch_0.9 .G4 Error .G5 Error in CASSIESystem .G5 .G4 Activating pitch_0.2 .G4 pitch_0.9 security systems in T minus pitch_2  5 . 4 . 3 . 2 . 1 . pitch_0.2 .G4 .G3 pitch_0.8 Protocol failed .G4 Site is now under .G4 Delta Command control bell_end",
            isNoisy: false,
            customSubtitles:
            "<color=red>Error Error</color> in <color=blue>CASSIESystem</color><split>Activating <color=blue>security systems</color> in <color=red>T-5 ... 4 ... 3 ... 2 ... 1 ...</color><split><color=red> Protocol failed .G4</color><split><color=blue>Site</color> is now under .G4 <color=green>Delta Command</color> control!"
        );

        Timing.RunCoroutine(FlickerLightsAsync());

        Cassie.Message(
            "The Chaos Insurgency has hacked the SCP Foundation's systems",
            isNoisy: false,
            customSubtitles: "The <color=green>Chaos Insurgency</color> has hacked the SCP Foundation's systems."
        );

        Timing.RunCoroutine(Hacked());
    }

    private static IEnumerator<float> FlickerLightsAsync()
    {
        for (var i = 0; i < 5; i++)
        {
            Map.TurnOffLights(1f);
            yield return Timing.WaitForSeconds(1);
        }
    }

    private static void ResetText()
    {
        Text.TextFormat = "<color=green>Idle...</color>";
    }

    private static IEnumerator<float> Hacked()
    {
        var mtf = Player.List.Where(x => x.Team == Team.FoundationForces).GetRandomElement();
        while (Round.IsRoundInProgress)
        {
            if (mtf != null && mtf.Team == Team.FoundationForces)
            {
                Text.TextFormat = $"<color=red>Hack has been completed</color>\nMTF Position: {mtf.CachedRoom.Name}";
            }
            else
            {
                mtf = Player.List.Where(x => x.Team == Team.FoundationForces).GetRandomElement();
                Text.TextFormat = "<color=red>Hack has been completed</color>\nNo MTF";
            }

            yield return Timing.WaitForSeconds(5);
        }
    }
}