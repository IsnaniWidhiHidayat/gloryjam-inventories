using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    #if ODIN_INSPECTOR
    [Serializable,HideReferenceObjectPicker]
    [Toggle("Enabled")]
    #endif
    public class ItemDismantleComponent : ItemComponent<ItemDismantleHandler,ItemDismantleComponent>
    {
        #region property
        public override string ComponentName => "Dismantle";
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

    public static class ItemDismantleComponentExtend
    {
        public static bool GetComponentDismantle(this ItemStack stack,out ItemDismantleComponent result){
            result = stack?.GetComponent<ItemDismantleComponent>();
            return result != null;
        }
        public static bool GetComponentDismantle(this Item item,out ItemDismantleComponent result){
            result = item?.GetComponent<ItemDismantleComponent>();
            return result != null;
        }

        public static bool ContainComponentDismantle(this ItemStack stack){
            return stack.ContainComponent<ItemDismantleComponent>();
        }
        public static bool ContainComponentDismantle(this Item item){
            return item.ContainComponent<ItemDismantleComponent>();
        }
    
    }
}
