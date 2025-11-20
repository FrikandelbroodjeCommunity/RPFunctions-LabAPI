// using System.Collections.Generic;
// using Exiled.API.Enums;
// using Exiled.API.Features;
// using Exiled.API.Features.Spawn;
// using Exiled.CustomRoles.API.Features;
// using PlayerRoles;
// using UnityEngine;
//
// namespace RPF.Events.CustomRoles.Humans
// {
//     public class Tech_Pro : CustomRole
//     {
//         public override uint Id { get; set; } = 103;
//         public override int MaxHealth { get; set; } = 100;
//         public override string Name { get; set; } = "Tech Pro";
//         public override string Description { get; set; } = "You are a technician! you know every room of the game!";
//         public override string CustomInfo { get; set; } = " Pro technician";
//         public override float SpawnChance { get; set; } = 75;
//         public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
//         public override List<string> Inventory { get; set; } = new List<string>()
//         {
//             "KeycardScientist",
//             "Medkit",
//             "Adrenaline",
//             "Flashlight",
//             "Radio"
//         };
//
//         public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
//         {
//             Limit = 1,
//             DynamicSpawnPoints = new List<DynamicSpawnPoint>()
//             {
//                 new DynamicSpawnPoint()
//                 {
//                     Chance = 100,
//                     Location = SpawnLocationType.InsideLczCafe
//                 }
//             }
//         };
//         
//
//         protected override void SubscribeEvents()
//         {
//             Debug.Log($"Role {Name} subscribed");
//             base.SubscribeEvents();
//         }
//
//         protected override void UnsubscribeEvents()
//         {
//             Debug.Log($"Role {Name} unsubscribed");
//             base.UnsubscribeEvents();
//         }
//
//         public override void AddRole(Player player)
//         {
//             //Patched: in 1.2.0
//             base.AddRole(player);
//             player.Broadcast(10, "You are a Expert Technician.");
//         }
//         
//     }
// }
//     
//
//
