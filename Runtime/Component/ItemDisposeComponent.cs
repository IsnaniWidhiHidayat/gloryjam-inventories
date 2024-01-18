#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace GloryJam.Inventories
{
    [DisallowMultipleItemComponent]
    public class ItemDisposeComponent : ItemComponent<ItemDisposeComponent>
    {
        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        [LabelWidth(200)]
        #endif
        public bool disposeWhenInventoryIsNull;
        #endregion

        #region property
        public override string name => "Dispose";
        public override int order => 101;
        public override bool showID => false;
        public override bool requiredId => false;
        #endregion

        #region callback
        public override void OnInit(){
            if(inventory == null && disposeWhenInventoryIsNull){
                stack.Dispose();
            }
        }
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}
