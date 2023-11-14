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
        protected const string grpStack = "Stack";
        protected const string grpConfig = "Config";
        protected const string grpRequired = "Required";
        protected const string grpRuntime = "Runtime";
        #endregion

        #region property
        public ItemStack stack => _stack;
        #endregion

        #region private
        #if ODIN_INSPECTOR
        [ShowInInspector,BoxGroup(grpStack),HideLabel]
        #endif
        protected ItemStack _stack;
        #endregion

        #region methods
        public void SetStack(ItemStack stack){
            _stack = stack;
        }
        #endregion
    }
}