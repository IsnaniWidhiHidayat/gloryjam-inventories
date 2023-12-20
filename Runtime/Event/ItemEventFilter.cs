using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemEventFilter
    {
        #region const
        const int labelWidth = 100;
        #endregion

        #region inner class
        public enum TypeInventory{
            All,
            ID,
        }
        public enum TypeItem{
            All,
            ItemAsset,
        }
        #endregion

        #region fields
        public TypeInventory typeInventory;

        #if ODIN_INSPECTOR
        [ShowIf(nameof(typeInventory),TypeInventory.ID)]
        #endif
        public string inventoryID;

        #if ODIN_INSPECTOR
        #endif
        public TypeItem typeItem;

        #if ODIN_INSPECTOR
        [ShowIf(nameof(typeItem),TypeItem.ItemAsset)]
        #endif
        public ItemAsset itemAsset;
        #endregion

        #region methods
        public bool IsPass(Inventory inventory,ItemStack stack){
            if(typeInventory == TypeInventory.ID){
                if(inventory == null) return false;
                if(inventory.id != inventoryID) return false;
            }

            if(typeItem == TypeItem.ItemAsset){
                if(stack == null) return false;
                if(stack.item == null) return false;
                if(stack.item.id != itemAsset.value.id) return false;
            }

            return true;
        }
        #endregion
    }
}