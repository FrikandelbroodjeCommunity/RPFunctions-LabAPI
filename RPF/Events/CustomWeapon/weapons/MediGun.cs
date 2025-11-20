// using Exiled.API.Features.Attributes;
// using Exiled.API.Features.Spawn;
// using Exiled.CustomItems.API.Features;
// using Exiled.Events.EventArgs.Player;
// using PlayerRoles;
// using Exiled.API.Enums;
// using UnityEngine;
// using Log = Exiled.API.Features.Log;
//
// namespace RPF.Events.CustomWeapon;
//
// [CustomItem(ItemType.GunCOM15)]
// public class MediGun : Exiled.CustomItems.API.Features.CustomWeapon
// {
//     public override ItemType Type { get; set; } = ItemType.GunCOM15;
//     public override uint Id { get; set; } = 140;
//     public override string Name { get; set; } = "Medi Gun";
//     public override string Description { get; set; } = null;
//     public override float Weight { get; set; } = 1f;
//     public override Vector3 Scale { get; set; } = new Vector3(1f, 1f, 1f);
//     public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();
//     public override float Damage { get; set; } = 0;
//
//
//     protected override void SubscribeEvents()
//     {
//         Exiled.Events.Handlers.Player.Shooting += OnPlayerShot;
//         base.SubscribeEvents();
//     }
//     protected override void UnsubscribeEvents()
//     {
//         Exiled.Events.Handlers.Player.Shooting -= OnPlayerShot;
//
//         base.UnsubscribeEvents();
//     }
//
//     public void OnPlayerShot(ShootingEventArgs ev)
//     {
//         if (!Check(ev.Item))
//             return;
//
//         if (ev.ClaimedTarget == null) return;
//         
//         
//         ev.ClaimedTarget.Heal(15);
//     }
// }