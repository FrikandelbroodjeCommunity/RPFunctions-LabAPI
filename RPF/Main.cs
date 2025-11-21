using System;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Loader.Features.Plugins;
using RPF.Events._914Event;
using RPF.Events.BroadCast;
using RPF.Events.Hack;
using RPF.Events.Misc;
using RPF.Events.RPSCP;
using RPF.Events.TeslaGate;

namespace RPF;

public class Main : Plugin<Config>
{
    public override string Name => "RPFunctions";
    public override string Description => "";
    public override string Author => "Mr.Cat";
    public override Version Version { get; } = new(1, 0, 0);
    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

    public static Main Instance { get; private set; }

    public override void Enable()
    {
        Instance = this;

        if (Config.Enable106Functions) Scp106DoorRestriction.RegisterEvents();
        if (Config.Enable096Functions) Scp096ElevatorRestriction.RegisterEvents();
        if (Config.Enable939Functions) Scp939DoorRestriction.RegisterEvents();
        if (Config.EnableFemurBreaker) FemurBreakerEvent.RegisterEvents();
        if (Config.StartAnnoucment) BroadCastBreach.RegisterEvents();
        if (Config.TeslaConditions) TeslaConditions.RegisterEvents();
        if (Config.Scp914Kill) Kill914.RegisterEvents();
        if (Config.UseHack) HackEvent.RegisterEvents();

        Assets.Load();

        Logger.Info("RPF enabled");
    }


    public override void Disable()
    {
        Scp106DoorRestriction.UnregisterEvents();
        Scp096ElevatorRestriction.UnregisterEvents();
        Scp939DoorRestriction.UnregisterEvents();
        FemurBreakerEvent.UnregisterEvents();
        BroadCastBreach.UnregisterEvents();
        TeslaConditions.UnregisterEvents();
        Kill914.UnregisterEvents();
        HackEvent.UnregisterEvents();

        Logger.Info("RPF disabled");
    }
}