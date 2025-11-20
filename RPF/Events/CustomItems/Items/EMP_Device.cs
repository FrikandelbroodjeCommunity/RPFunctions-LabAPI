// using System;
// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;
// using UnityEngine;
//
// namespace RPF.Events.CustomItems.Items;
//
// public class EMP_Device : CustomItem
// {
//     public override uint Id { get; set; } = 101;
//     public override string Name { get; set; } = "EMP Device";
//     public override string Description { get; set; } = "With This device you can close all Lights.";
//     public override float Weight { get; set; } = 1.5f;
//     public override ItemType Type { get; set; } = ItemType.KeycardChaosInsurgency;
//
//     public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
//     {
//         Limit = 2,
//         DynamicSpawnPoints = new List<DynamicSpawnPoint>()
//         {
//             new DynamicSpawnPoint()
//             {
//                 Chance = 100,
//                 Location = SpawnLocationType.InsideLczArmory
//             }
//         }
//     };
//         
//     private const int DURATION_SECONDS = 60;
//          
//     private static volatile bool _isActive = false;
//     private static DateTime _empEndUtc = DateTime.MinValue;
//         
//     private static DateTime _nextAvailableUtc = DateTime.MinValue;
//         
//     private static CancellationTokenSource _cts;
//
//     protected override void SubscribeEvents()
//     {
//         Exiled.Events.Handlers.Player.PickingUpItem += OnPickup;
//         Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
//         Log.Debug($"Item {Name} Subscribed");
//         base.SubscribeEvents();
//     }
//
//     protected override void UnsubscribeEvents()
//     {
//         Exiled.Events.Handlers.Player.PickingUpItem -= OnPickup;
//         Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
//         Log.Debug($"Item {Name} Unsubscribed");
//         CancelCountdownAndRestore();
//         base.UnsubscribeEvents();
//     }
//
//     private void OnPickup(PickingUpItemEventArgs ev)
//     {
//         try
//         {
//             if (ev?.Pickup == null) return;
//             if (ev.Pickup.Type != Type) return;
//
//             var now = DateTime.UtcNow;
//             if (now < _nextAvailableUtc)
//             {
//                 var rem = (int)Math.Ceiling((_nextAvailableUtc - now).TotalSeconds);
//                 ev.Player.ShowHint($"<color=yellow>EMP Recharging: {rem}s</color>", 3);
//                 return;
//             }
//                 
//             _nextAvailableUtc = now.AddSeconds(DURATION_SECONDS);
//             _empEndUtc = now.AddSeconds(DURATION_SECONDS);
//                 
//             _ = ActivateEmpWithCountdown();
//         }
//         catch (Exception ex)
//         {
//             Log.Error($"[EMPDevice] OnPickup error: {ex}");
//         }
//     }
//
//     private async Task ActivateEmpWithCountdown()
//     {
//         CancelCountdown();
//         _cts = new CancellationTokenSource();
//         var ct = _cts.Token;
//
//         try
//         {
//             _isActive = true;
//             Log.Info("[EMPDevice] EMP activated");
//                 
//             foreach (var p in Player.List)
//                 p.ShowHint("<color=yellow>EMP actived.</color>", 5);
//
//                 
//             try { Map.ChangeLightsColor(Color.black); }
//             catch (Exception ex) { Log.Error($"[EMPDevice] ChangeLightsColor error: {ex}"); }
//
//                 
//             for (int remaining = DURATION_SECONDS; remaining >= 0; remaining--)
//             {
//                 if (ct.IsCancellationRequested) break;
//
//                 foreach (var p in Player.List)
//                     p.ShowHint($"<color=red>EMP active - recharging in: {remaining}s</color>", 1);
//
//                 try { await Task.Delay(1000, ct); }
//                 catch (TaskCanceledException) { break; }
//             }
//
//                 
//             try { Map.ChangeLightsColor(Color.white); }
//             catch (Exception ex) { Log.Error($"[EMPDevice] ChangeLightsColor restore error: {ex}"); }
//                 
//             foreach (var p in Player.List)
//                 p.ShowHint("<color=green>Devices restarted.</color>", 5);
//
//             Cassie.Message("ELETRONIC DEVICED RESTARTED.", isNoisy: false, isSubtitles: true);
//
//             Log.Info("[EMPDevice] EMP finished and lights restored.");
//         }
//         catch (Exception ex)
//         {
//             Log.Error($"[EMPDevice] Activation error: {ex}");
//             try { Map.ChangeLightsColor(Color.white); } catch { }
//         }
//         finally
//         {
//             _isActive = false;
//             CancelCountdown();
//         }
//     }
//
//         
//     private void OnInteractingDoor(InteractingDoorEventArgs ev)
//     {
//         try
//         {
//             if (ev == null || ev.Player == null) return;
//
//             if (!_isActive) return;
//
//                
//             var now = DateTime.UtcNow;
//             int remaining = Math.Max(0, (int)Math.Ceiling((_empEndUtc - now).TotalSeconds));
//
//                 
//             ev.IsAllowed = false;
//             ev.Player.ShowHint($"<color=red>EMP active Time remaining: {remaining}s</color>", 3);
//         }
//         catch (Exception ex)
//         {
//             Log.Error($"[EMPDevice] OnInteractingDoor error: {ex}");
//         }
//     }
//
//     private static void CancelCountdownAndRestore()
//     {
//         try
//         {
//             _cts?.Cancel();
//             _cts?.Dispose();
//             _cts = null;
//         }
//         catch { /* ignore */ }
//
//         try { Map.ChangeLightsColor(Color.white); } catch { }
//         _isActive = false;
//         _empEndUtc = DateTime.MinValue;
//     }
//
//     private static void CancelCountdown()
//     {
//         try
//         {
//             _cts?.Cancel();
//             _cts?.Dispose();
//             _cts = null;
//         }
//         catch { /* ignore */ }
//     }
// }