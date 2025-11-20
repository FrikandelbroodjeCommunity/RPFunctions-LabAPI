using System.ComponentModel;

namespace RPF
{
    public class Config
    {
        [Description("----------------------- Pl Main -----------------------")]
        public bool Debug { get; set; } = false;

        [Description("----------------------- SCP RP EVENTS ----------------------")]
        public string ScpRpFunctions096 { get; set; } = "You can't use elevators when you are not in rage!";
        public bool Enable106Functions { get; set; } = true;
        public string ScpRpFunctions106 { get; set; } = "You can't use doors, ONLY elevators!";
        public bool Enable096Functions { get; set; } = true;
        public string ScpRpFunctions939 { get; set; } = "You can't use elevators!";
        public bool Enable939Functions { get; set; } = true;
        
        [Description("----------------------- FemurBreaker -----------------------")]
        public bool EnableFemurBreaker { get; set; } = true;
        public string FemurCommand { get; set; } = "femur";
        public int GeneratorsRequired { get; set; } = 3;
        public bool OnlyHumansCanTrigger { get; set; } = true;
        public int FemurBreakerDelay { get; set; } = 8000;
        public static string FemurBreakerCassie { get; set; } = "<b><color=red>Femur Breaker Actived . . .</color></b>";
        
        [Description("------------------------ Overload Command ---------------------")]
        public string OverloadCommand { get; set; } = "Overload";
        public bool EnableOverloadCommand { get; set; } = true;
        public string Overload079Cassie { get; set; } = "Overload... Completed...";
        
        [Description("------------------------ Scientist Command ------------------------")]
        public string ScientistInstructions { get; set; } = "Go to the Exit here a keycard to help!";

        [Description("------------------------- BradCastBreach Main ---------------------")]
        public bool StartAnnoucment { get; set; } = true;
        
        [Description("------------------------- Tesla Conditions -------------------------")]
        public bool TeslaConditions { get; set; } = true;
        
        [Description("------------------------- SCP 914 kills -------------------------")]
        public bool Scp914Kill { get; set; } = true;
        
        [Description("------------------------- Custom Roles & Items ----------------------")]
        public bool CustomRoles { get; set; } = true;
        public bool CustomItems { get; set; } = true;
        
    }
}