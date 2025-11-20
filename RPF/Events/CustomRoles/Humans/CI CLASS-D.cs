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
//     public class CI_CLASS_D : CustomRole
//     {
//         public override uint Id { get; set; } = 107;
//         public override int MaxHealth { get; set; } = 100;
//         public override string Name { get; set; } = "CI CLASS D";
//         public override string Description { get; set; } = "You are a CI Employee and you know everything on the facility.";
//         public override string CustomInfo { get; set; } = "Class-D";
//         public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
//         public override float SpawnChance { get; set; } = 100;
//         
//         public override List<string> Inventory { get; set; } = new List<string>()
//         {
//             "KeycardChaosInsurgency",
//             "Medkit",
//             "Flashlight"
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
//                     Location = SpawnLocationType.InsideLczWc
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
//             player.Broadcast(10, "You are a CI CLASS-D. shhh...");
//         }
//         
//     }
// }
//     
//
