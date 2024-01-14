using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    public class ItemSetComponent : ItemComponent<ItemSetComponent>
    {
        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpRequired),InlineEditor,Required]
        #endif
        public ItemSetDataAsset itemSet;
        #endregion

        #region property
        public override string name => "Set";
        public override int order => 102;
        #endregion

        #region methods
        public override ItemComponent CreateInstance()
        {
            return this;
        }
        #endregion

        #region callback
        public override void OnInit()
        {
            itemSet?.value.OnItemStackInit(stack);
        }
        public override void OnPostInit(){}
        public override void OnDispose()
        {
            itemSet?.value.OnItemStackDispose(stack);
        }
        #endregion
    }
}
