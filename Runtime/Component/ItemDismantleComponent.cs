using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    [DisallowMultipleItemComponent]
    public class ItemDismantleComponent : ItemComponent<ItemDismantleComponent,ItemDismantleHandler>
    {
        #region property
        public override string name => "Dismantle";
        public override int propertyOrder => 1;
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
