using HarmonyLib;
using Interactables.Interobjects.DoorUtils;

namespace RPF.Events.Misc;

[HarmonyPatch]
public static class FemurBreakerPatch
{
    [HarmonyPatch(typeof(DoorVariant), nameof(DoorVariant.NetworkTargetState), MethodType.Setter)]
    [HarmonyPrefix]
    // ReSharper disable once InconsistentNaming
    public static bool NoOpeningBeforeGens(DoorVariant __instance)
    {
        if (__instance.DoorName == "106_PRIMARY" || __instance.DoorName == "106_SECONDARY")
        {
            return FemurBreakerEvent.DoorUnlockedByGenerators;
        }

        return true;
    }
}