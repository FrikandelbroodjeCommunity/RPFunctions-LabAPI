// using RPF.Events.CustomItems.Items;
//
// //Put new Handler for version 1.3.0
// namespace RPF.Events.CustomItems;
//
// public class CustomItemsHandler
// {
//     public void Register()
//     {
//         if (Main.Instance.Config.CustomItems) return;
//         new EMP_Device().Register();
//         new SuperAdrenaline().Register();
//     }
//
//     public void Unregister()
//     {
//         EMP_Device.UnregisterItems();
//         SuperAdrenaline.UnregisterItems();
//     }
// }