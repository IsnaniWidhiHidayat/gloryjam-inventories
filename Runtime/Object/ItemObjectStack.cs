using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/" + nameof(ItemObjectStack))]
    public class ItemObjectStack : MonoBehaviour ,IItemObjectStack
    {
        #region property
        #if ODIN_INSPECTOR
        [ShowInInspector,HideLabel,HideDuplicateReferenceBox,HideReferenceObjectPicker]
        #endif
        public Item item => stack != null? stack.item : default;

        #if ODIN_INSPECTOR
        [ShowInInspector,HideLabel,HideDuplicateReferenceBox,HideReferenceObjectPicker]
        #endif
        public ItemStack stack {
            get => _stack;
            set => _stack = value;
        }
        #endregion

        #region protected
        protected ItemStack _stack;
        #endregion
    }
}