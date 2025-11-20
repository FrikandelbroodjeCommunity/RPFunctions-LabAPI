using System;
using System.Linq;
using CommandSystem;
using LabApi.Features.Wrappers;
using MEC;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace RPF.Commands.RA;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class OmegaWarhead : ICommand
{
    public string Command { get; } = "OmegaWarhead";
    public string[] Aliases => new[] { "OmegaWarhead" };
    public string Description { get; } = "Starts the legendary Omega Warhead";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "Omega Warhead is running...";
        Extetic();
        Timing.CallDelayed(Warhead.DetonationTime, Part);
        return true;
    }

    private static void Extetic()
    {
        try
        {
            Warhead.Start();
            Warhead.IsLocked = true;
            Map.SetColorOfLights(Color.blue);
        }
        catch (Exception ex)
        {
            Logger.Error($"Omega Warhead exception occured: {ex}");
        }
    }

    private void Part()
    {
        if (Warhead.IsDetonated)
        {
            Warhead.Shake();
        }
        else
        {
            Warhead.Detonate();
        }

        foreach (var ply in Player.List.Where(x => x.IsAlive))
        {
            ply.Kill("Vaporized by Omega Warhead.");
        }

        Logger.Info("Omega Warhead has been detonated.");
    }
}