using System;
using GloryJam.DataAsset;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using UnityEngine;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemSetData
    {
        #region fields
        #if ODIN_INSPECTOR
        [TableList(AlwaysExpanded = true)]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public ItemReferenceCount [] items = new ItemReferenceCount [0];

        #if ODIN_INSPECTOR
        [ListDrawerSettings(DraggableItems = false,Expanded = true,ListElementLabelName = "title")]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public ItemSetHandler[] handler = new ItemSetHandler[0];
        #endregion
    }

    [CreateAssetMenu(menuName = "Glory Jam/Inventory/Item Set Data")]
    public class ItemSetDataAsset : DataAsset<ItemSetData>{}
}
