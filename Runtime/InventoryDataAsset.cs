using System;
using GloryJam.DataAsset;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif


namespace GloryJam.Inventories
{
    [Serializable]
    public class InventoryData
    {
        #region fields
        #if ODIN_INSPECTOR
        [ListDrawerSettings(IsReadOnly = true,ListElementLabelName = "title")]
        [NonSerialized,ShowInInspector]
        [HideDuplicateReferenceBox,HideReferenceObjectPicker]
        #endif
        public ItemSlot[] slots;
        #endregion

        #region constructor
        public InventoryData(){
            slots = new ItemSlot[0];
        }
        #endregion
    }

    [CreateAssetMenu(menuName = "Glory Jam/Inventory/" + nameof(InventoryDataAsset))]
    public class InventoryDataAsset : DataAsset<InventoryData>{}
}