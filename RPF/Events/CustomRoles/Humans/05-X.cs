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
//     public class O5X : CustomRole
//     {
//         public override uint Id { get; set; } = 101;
//         public override int MaxHealth { get; set; } = 500;
//         public override string Name { get; set; } = "O5-X";
//         public override string Description { get; set; } = "You are the O5-X. You are the highest role in the game!";
//         public override string CustomInfo { get; set; } = "O5-X";
//         public override float SpawnChance { get; set; } = 50;
//         public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;
//         
//         public override List<string> Inventory { get; set; } = new List<string>()
//         {
//             "KeycardO5",
//             "Medkit",
//             "Radio",
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
//                     Location = SpawnLocationType.InsideIntercom
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
//             player.Broadcast(10, "You are a O5-X!");
//         }
//         
//     }
// }
//         
//         
//         
//     
//
//
