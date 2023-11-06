using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    #if ODIN_INSPECTOR
    [Serializable,HideReferenceObjectPicker,HideDuplicateReferenceBox]
    [Toggle("Enabled",CollapseOthersOnExpand = false)]
    #endif
    [DisallowMultipleItemComponent]
    public class ItemDismantleComponent : ItemComponent<ItemDismantleComponent,ItemDismantleHandler>
    {
        #region property
        public override string ComponentName => "Dismantle";
        public override int ComponentPropertyOrder => 1;
        #endregion

        #region methods
        public bool Dismantle(ref Dictionary<Item, int> result)
        {
            var r = true;
            
            for (int i = 0; i < handlers.Count; i++)
            {
                r &= handlers[i].Dismantle(ref result);
            }

            return r;
        }
        #endregion
    }
}
