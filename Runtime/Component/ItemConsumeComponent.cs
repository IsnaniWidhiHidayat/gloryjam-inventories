using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    #if ODIN_INSPECTOR
    [Serializable,HideReferenceObjectPicker,HideDuplicateReferenceBox]
    [Toggle("Enabled")]
    #endif
    [DisallowMultipleItemComponent]
    public class ItemConsumeComponent : ItemComponent<ItemConsumeComponent>
    {
        #region property
        public override string ComponentName => "Consume";
        public override int ComponentPropertyOrder => 100;
        #endregion

        #region methods
        #if ODIN_INSPECTOR
        [Button]
        #endif
        public void Consume(){
            stack.Dispose();
        }
        public override void LoadState(){}
        public override void SaveState(){}
        public override void Dispose(){}
        #endregion
    }
}
