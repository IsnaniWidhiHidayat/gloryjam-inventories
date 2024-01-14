using System;
using System.Collections.Generic;
using GloryJam.DataAsset;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class InventorySaveData : IDataReset
    {
        #region fields
        public int maxSlot;

        #if ODIN_INSPECTOR
        [ListDrawerSettings(DraggableItems = false,Expanded = true,ListElementLabelName = "id")]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public List<ItemSaveData> items = new List<ItemSaveData>();
        #endregion

        #region constructor
        public void Reset()
        {
            if(items == null) items = new List<ItemSaveData>();
            items.Clear();
        }
        #endregion
    }

    [Serializable]
    public class ItemSaveData 
    {
        #region fields
        public string id;
        public int index;

        #if ODIN_INSPECTOR
        [ListDrawerSettings(DraggableItems = false,ShowIndexLabels = true)]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public List<ItemStackSaveData> stack = new List<ItemStackSaveData>();
        #endregion
    }

    [Serializable]
    public class ItemStackSaveData{

        public int hash;

        #if ODIN_INSPECTOR
        [ListDrawerSettings(DraggableItems = false,Expanded = true,ListElementLabelName = "id")]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public List<ItemStateSaveData> state = new List<ItemStateSaveData>();
    }

    [Serializable]
    public abstract class ItemStateSaveData{
        [Required]
        public string id;
    }

    [CreateAssetMenu(menuName = "Glory Jam/Inventory/Inventory Save Data")]
    public class InventorySaveDataAsset : DataAsset<InventorySaveData>{}
}

