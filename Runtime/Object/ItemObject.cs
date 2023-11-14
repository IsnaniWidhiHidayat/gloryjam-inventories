// using UnityEngine;

// #if ODIN_INSPECTOR
// using Sirenix.OdinInspector;
// #endif

// namespace GloryJam.Inventories
// {
//     public class ItemObject : MonoBehaviour 
//     {
//         #region const
//         const string grpConfig = "Config";
//         const string grpRuntime = "Runtime";
//         const string grpRequired = "Required";
//         #endregion

//         #region property
//         #if ODIN_INSPECTOR
//         [ShowInInspector,BoxGroup(grpRuntime)]
//         #endif
//         public ItemStack stack => _stack;
//         #endregion

//         #region private
//         private ItemStack _stack;
//         #endregion

//         #region methods
//         public void SetStack(ItemStack stack){
//             _stack = stack;
//         }
//         #endregion
//     }
// }