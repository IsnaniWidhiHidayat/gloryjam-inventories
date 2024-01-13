using Sirenix.OdinInspector;
using UnityEngine;

namespace GloryJam.Inventories
{
    [DisallowMultipleItemComponent]
    public class ItemDisposeComponent : ItemComponent<ItemDisposeComponent>
    {
        #region fields
        [BoxGroup(grpConfig),LabelWidth(200)]
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
