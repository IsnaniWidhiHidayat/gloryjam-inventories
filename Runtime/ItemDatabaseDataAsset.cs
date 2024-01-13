using System;
using System.Collections.Generic;
using GloryJam.DataAsset;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemDatabaseData 
    {
        #region fields
        [ListDrawerSettings(Expanded = true,DraggableItems = false)]
        public List<ItemAsset> items = new List<ItemAsset>();
        #endregion

        #region methods
        public bool TryGetItem(string id, out Item item)
        {
            item = items.Find(x=> x != null && x.value.id == id)?.value;
            return item != null;
        }
        #endregion
    }

    [CreateAssetMenu(menuName = "Glory Jam/Inventory/Item Database Data Asset")]
    public class ItemDatabaseDataAsset : DataAsset<ItemDatabaseData>
    {
        
    }
}