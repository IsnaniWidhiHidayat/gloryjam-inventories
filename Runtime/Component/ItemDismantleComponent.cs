using System;
using System.Collections.Generic;
using UnityEngine;

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
        public override int order => 1;
        public override bool showID => false;
        public override bool requiredId => false;
        #endregion

        #region methods
        public bool Dismantle(ref Dictionary<Item, int> result)
        {
            Debug.Log($"[Inventory]Item Dismantle, stack:{stack}");

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
