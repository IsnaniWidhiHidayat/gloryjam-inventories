using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/" + nameof(ItemObjectStack))]
    public class ItemObjectStack : MonoBehaviour ,IItemObjectStack
    {
        #region const
        protected const string grpItem = "Item";
        protected const string grpConfig = "Config";
        protected const string grpRequired = "Required";
        protected const string grpRuntime = "Runtime";
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [ShowInInspector,BoxGroup(grpItem),HideLabel,HideDuplicateReferenceBox,HideReferenceObjectPicker]
        #endif
        protected Item _item;

        #if ODIN_INSPECTOR
        [ShowInInspector,BoxGroup(grpItem),HideLabel,HideDuplicateReferenceBox,HideReferenceObjectPicker]
        #endif
        protected ItemStack _stack;
        #endregion

        #region property
        public Item item => _stack?.item;
        public ItemStack stack => _stack;
        #endregion

        #region methods
        public void SetStack(ItemStack stack){
            _stack = stack;
            _item = stack?.item;
        }
        #endregion
    }
}