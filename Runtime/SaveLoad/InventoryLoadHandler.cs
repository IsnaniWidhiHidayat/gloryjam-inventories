using UnityEngine;
using GloryJam.SaveLoad;
using GloryJam.DataAsset;
using Sirenix.OdinInspector;

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/Inventory Load Handler")]
    public class InventoryLoadHandler : LoadHandler<InventorySaveData>
    {
        #region private
        [BoxGroup(grpSaveData),SerializeField]
        private DataReference<InventorySaveData> _saveData;

        [BoxGroup(grpRequired),SerializeField,Required,InlineEditor]
        private InventoryDataAsset _inventoryData;
        #endregion

        #region methods
        public override void Load(InventorySaveData saveData)
        {
            var inventoryData = _inventoryData;

            //reset
            _saveData.Reset();

            //copy items
            _saveData.value.items = saveData.items;
            _saveData.value.maxSlot = saveData.maxSlot;

            //clear inventory
            for (int i = 0; i < inventoryData.value.slots.Length; i++)
            {
                inventoryData.value.slots[i]?.Dispose();
            }

            //set max slot;
            inventoryData.value.slots = new ItemSlot[_saveData.value.maxSlot];

            //add item to inventory
            for (int i = 0; i < _saveData.value.items.Count; i++)
            {
                var itemSaveData = _saveData.value.items[i];

                //remove empty stack
                itemSaveData.stack.RemoveAll(stack => {
                    if(stack != null && stack.state != null){
                        stack.state.RemoveAll((state)=> state == null);
                    }
                    return stack == null || stack.state == null || stack.state.Count <= 0;
                });

                //check empty data
                if(itemSaveData == null) continue;

                //check empty id
                if(string.IsNullOrEmpty(itemSaveData.id)) continue;

                //check empty stack
                if(itemSaveData.stack == null || itemSaveData.stack.Count <= 0) continue;

                //Get item from item database
                if(!ItemDatabase.current.TryGetItem(itemSaveData.id,out Item item)) continue;

                //create item slot
                var slot = inventoryData.value.slots[itemSaveData.index] = new ItemSlot(item,null);

                //add item stack
                slot.Add(itemSaveData.stack.Count);

                //set state stack
                for (int j = 0; j < slot.stack.Count; j++)
                {
                    var stack = slot.stack[j];
                    var stackSaveData = itemSaveData.stack[j];

                    //check empty stack & data
                    if(stack == null || stackSaveData == null) continue;

                    //set stack hash
                    stack.hash = stackSaveData.hash;

                    //check state
                    if(stackSaveData.state == null || stackSaveData.state.Count <= 0) continue;

                    //check contain state component
                    if(!stack.TryGetComponentState(out var stateComponent)) continue;

                    //check handlers count
                    if(stateComponent.handlers == null || stateComponent.handlers.Count <= 0) continue;

                    //set handler state
                    for (int k = 0; k < stackSaveData.state.Count; k++)
                    {
                        var stateSaveData = stackSaveData.state[k];

                        //check empty state save data
                        if(stateSaveData == null) continue;

                        //check empty state handlerID
                        if(string.IsNullOrEmpty(stateSaveData.id)) continue;

                        //find handler by handlerID
                        var handler = stateComponent.handlers.Find(x=> x.id == stateSaveData.id);
                        if(handler == null) continue;

                        handler.saveData = stateSaveData;
                    }
                }
            }
        }
        // private void Reset() {
        //     if(_inventory == null) _inventory.GetComponent<Inventory>();
        // }
        public override string ToString()
        {
            return name;
        }
        #endregion
    }
}

