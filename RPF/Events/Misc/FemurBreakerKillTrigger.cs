using System;
using FrikanUtils.ProjectMer;
using LabApi.Features.Wrappers;
using UnityEngine;

namespace RPF.Events.Misc;

public class FemurBreakerKillTrigger : TriggerPrimitive
{
    [NonSerialized] public bool HasSacrifice;
    [NonSerialized] public Transform Door;
    [NonSerialized] public Vector3 OriginalPos;

    private void OnTriggerEnter(Collider other)
    {
        if (HasSacrifice) return;

        var player = Player.Get(other.gameObject);
        if (player == null) return;

        player.Kill("You were sacrificed for the recontainment of SCP-106");
        HasSacrifice = true;
        Door.position = OriginalPos;
    }
}