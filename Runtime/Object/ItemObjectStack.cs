using UnityEngine;
using System;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/Item Object Stack")]
    public class ItemObjectStack : MonoBehaviour ,IItemObjectStack
    {
        #region const
        const string grpItem = "Stack";
        #endregion

        #region property
        #if ODIN_INSPECTOR
        [ShowInInspector,BoxGroup(grpItem),HideLabel,HideDuplicateReferenceBox,HideReferenceObjectPicker]
        #endif
        public ItemStack stack {
            get => _stack;
            set => _stack = value;
        }
        #endregion

        #region protected
        [NonSerialized]
        protected ItemStack _stack;
        #endregion
    }
}